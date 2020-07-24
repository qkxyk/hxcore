using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HXCloud.Service
{
    public class TypeSchemaService : ITypeSchemaService
    {
        private readonly ILogger<TypeSchemaService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeSchemaRepository _ts;
        private readonly ITypeDataDefineRepository _td;
        private readonly ITypeRepository _tr;

        public TypeSchemaService(ILogger<TypeSchemaService> log, IMapper mapper, ITypeSchemaRepository ts, ITypeDataDefineRepository td, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._ts = ts;
            this._td = td;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeSchemaModel, bool>> predicate)
        {
            var data = await _ts.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeSchemaModel, bool>> predicate, out string groupId)
        {
            bool bRet = false;
            string gId = "";
            var data = _ts.FindWithType(predicate).Result;
            if (data == null)
            {
                bRet = false;
            }
            else
            {
                bRet = true;
                gId = data.Type.GroupId;
            }
            groupId = gId;
            return bRet;
        }
        public async Task<BaseResponse> AddSchemaAsync(int typeId, TypeSchemaAddViewModel req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            //检查是否存在同名
            var data = await _ts.Find(a => a.TypeId == typeId && a.ParentId == req.ParentId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的模式" };
            }
            var define = await _td.FindAsync(req.DataDefineId);
            //检查所对应的key值是否存在
            if (define == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据定义不存在" };
            }
            try
            {
                if (req.ParentId == 0)
                {
                    req.ParentId = null;
                }
                var entity = _mapper.Map<TypeSchemaModel>(req);
                entity.Create = account;
                entity.TypeId = typeId;
                entity.Key = define.DataKey;
                await _ts.AddAsync(entity);
                _log.LogInformation($"{account}添加模式成功，模式标示为{entity.Id}");
                return new HandleResponse<int> { Success = true, Message = "添加模式成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加模式失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加模式失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> Delete(int Id, string account)
        {
            var entity = await _ts.FindWithChild(Id);
            if (entity == null)
            {
                return new BaseResponse { Success = false, Message = "输入的模式不存在" };
            }
            else if (entity.Child.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "该模式下存在子模式，不能删除" };
            }
            try
            {
                await _ts.RemoveAsync(entity);
                _log.LogInformation($"{account}删除标示为{Id}的模式成功");
                return new BResponse<int> { Success = true, Message = "删除模式成功", Data = Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的模式失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除模式失败，请联系管理员" };
            }
        }
        public async Task GetChild(TypeSchemaData td, int Id)
        {
            var ret = await _ts.Find(a => a.ParentId == Id).ToListAsync();
            foreach (var item in ret)
            {
                var dto = _mapper.Map<TypeSchemaData>(item);
                td.Child.Add(dto);
                await GetChild(dto, item.Id);
            }
        }
        //根据模式编号获取模式信息
        public async Task<BaseResponse> GetSchemaById(int Id)
        {
            var data = await _ts.FindWithChild(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的模式编号不存在" };
            }
            var dto = _mapper.Map<TypeSchemaData>(data);
            if (data.Child.Count > 0)
            {
                await GetChild(dto, Id);
            }
            return new BResponse<TypeSchemaData> { Success = true, Message = "获取模式数据成功", Data = dto };
        }
        //根据类型编号获取模式信息
        public async Task<BaseResponse> GetTypeSchema(int TypeId)
        {
            var data = await _ts.Find(a => a.TypeId == TypeId && a.Parent == null).ToListAsync();
            List<TypeSchemaData> list = new List<TypeSchemaData>();

            foreach (var item in data)
            {
                var dto = _mapper.Map<TypeSchemaData>(item);
                await GetChild(dto, item.Id);
                list.Add(dto);
            }
            return new BResponse<List<TypeSchemaData>> { Success = true, Message = "获取模式数据成功", Data = list };
        }

        public async Task<BaseResponse> Update(TypeSchemaUpdateViewModel req, string account)
        {
            var data = await _ts.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "该模式不存在" };
            }
            var ret = await _ts.Find(a => a.ParentId == data.ParentId && a.TypeId == data.TypeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (ret != null)
            {
                if (ret.Id == data.Id)
                {
                    return new BaseResponse { Success = true, Message = "修改模式名称成功" };
                }
                else
                {
                    return new BaseResponse { Success = false, Message = "已存在相同的模式名称" };
                }
            }
            try
            {
                var dto = _mapper.Map(req, data);
                dto.Modify = account;
                dto.ModifyTime = DateTime.Now;
                await _ts.SaveAsync(dto);
                _log.LogInformation($"{account}修改标示为{req.Id}的模式的名称成功");
                return new BResponse<int> { Success = true, Message = "修改模式名称成功", Data = req.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的模式名称失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }

        }
    }
}
