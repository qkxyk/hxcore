using System;
using System.Collections.Generic;
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
    public class TypeSystemService : ITypeSystemService
    {
        private readonly ILogger<TypeSchemaService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeSystemRepository _tsr;
        private readonly ITypeRepository _tr;

        public TypeSystemService(ILogger<TypeSchemaService> log, IMapper mapper, ITypeSystemRepository tsr, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._tsr = tsr;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeSystemModel, bool>> predicate)
        {
            var data = await _tsr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeSystemModel, bool>> predicate, out string GroupId)
        {
            var data = _tsr.FindWithType(predicate).Result;
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.Type.GroupId;
            return true;
        }

        public async Task<BaseResponse> AddTypeSystemAsync(int typeId, TypeSystemAddDto req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            var data = await _tsr.Find(a => a.TypeId == typeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下存在相同名称的子系统" };
            }
            try
            {
                var entity = _mapper.Map<TypeSystemModel>(req);
                entity.Create = account;
                entity.TypeId = typeId;
                await _tsr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的类型子系统成功");
                return new HandleResponse<int> { Success = true, Message = "添加子系统成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型子系统失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型子系统失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateTypeSystemAsync(int typeId, TypeSystemUpdateDto req, string account)
        {
            var data = await _tsr.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            }
            var sys = await _tsr.Find(a => a.TypeId == typeId && a.Name == req.Name).FirstOrDefaultAsync();
            if (sys != null && sys.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同名称的子系统" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _tsr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的类型子系统成功");
                return new BaseResponse { Success = false, Message = "修改类型子系统成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的类型子系统失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型子系统失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteTypeSystemAsync(int Id, string account)
        {
            var data = await _tsr.FindWithAccessory(a => a.Id == Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            }
            if (data.SystemAccessories.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "该子系统下存在配件，不能删除" };
            }
            try
            {
                await _tsr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的子系统成功");
                return new BaseResponse { Success = false, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的子系统失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetSystemAsync(int Id)
        {
            var data = await _tsr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的系统编号不存在" };
            }
            var dto = _mapper.Map<TypeSystemDto>(data);
            return new BResponse<TypeSystemDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetTypeSystemAsync(int typeId)
        {
            var data = await _tsr.Find(a => a.TypeId == typeId).ToListAsync();
            var dto = _mapper.Map<List<TypeSystemDto>>(data);
            return new BResponse<List<TypeSystemDto>> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
