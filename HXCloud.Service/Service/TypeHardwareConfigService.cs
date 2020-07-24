using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
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
    public class TypeHardwareConfigService : ITypeHardwareConfigService
    {
        private readonly ILogger<TypeHardwareConfigService> _log;
        private readonly IMapper _mapper;
        private readonly ITypeHardwareConfigRepository _th;
        private readonly ITypeDataDefineRepository _tdr;
        private readonly ITypeRepository _tr;

        public TypeHardwareConfigService(ILogger<TypeHardwareConfigService> log, IMapper mapper, ITypeHardwareConfigRepository th, ITypeDataDefineRepository tdr, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._th = th;
            this._tdr = tdr;
            this._tr = tr;
        }
        public async Task<bool> IsExist(Expression<Func<TypeHardwareConfigModel, bool>> predicate)
        {
            var data = await _th.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public bool IsExist(Expression<Func<TypeHardwareConfigModel, bool>> predicate, out string GroupId)
        {
            var data = _th.FindWithType(predicate).Result;
            if (data == null)
            {
                GroupId = null;
                return false;
            }
            GroupId = data.Type.GroupId;
            return true;
        }

        //主要用于导入到设备中
        public async Task<List<TypeHardwareConfigModel>> GetTypeHardwareConfigAsync(int TypeId)
        {
            var data = await _th.Find(opt => opt.TypeId == TypeId).ToListAsync();
            return data;
        }

        public async Task<BaseResponse> AddTypeHarewareConfigAsync(int typeId, TypeHardwareConfigAddDto req, string account)
        {
            //验证类型是否可以添加
            var t = await _tr.FindAsync(typeId);
            if (t.Status == TypeStatus.Root)
            {
                return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            }
            var dataDefine = await _tdr.FindAsync(req.DataDefineId);
            if (dataDefine == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据定义编号不存在" };
            }
            var data = await _th.Find(a => a.TypeId == typeId && a.Key == dataDefine.DataKey).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该类型下已存在相同的配置数据" };
            }
            try
            {
                var entity = _mapper.Map<TypeHardwareConfigModel>(req);
                entity.TypeId = typeId;
                entity.Create = account;
                entity.Key = dataDefine.DataKey;
                entity.KeyName = dataDefine.DataName;
                entity.Unit = dataDefine.Unit;
                entity.Format = dataDefine.Format;
                await _th.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为:{entity.Id}的类型PLC配置数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加配置数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加类型PLC配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加类型PLC配置数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateTypeHardwareConfigAsync(int typeId, TypeHardwareConfigUpdateDto req, string account)
        {
            var data = await _th.FindAsync(req.Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "该类型下不存在该PLC配置数据" };
            }
            try
            {
                var entity = _mapper.Map(req, data);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _th.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的PLC配置数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的PLC配置数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改类型PLC配置数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> DeleteTypeHardwareConfigAsync(int Id, string account)
        {
            var data = await _th.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型PLC配置数据不存在" };
            }
            try
            {
                await _th.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的类型PLC配置数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的类型PLC配置数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetHardwareConfigAsync(int Id)
        {
            var data = await _th.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的类型PLC配置数据不存在" };
            }
            var dto = _mapper.Map<TypeHardwareConfigDto>(data);
            return new BResponse<TypeHardwareConfigDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetTypeHardwareConfigAsync(int typeId, BasePageRequest req)
        {
            var query = _th.Find(a => a.TypeId == typeId);
            if (!string.IsNullOrWhiteSpace(req.Search))
            {
                query = query.Where(a => a.KeyName.Contains(req.Search) || a.Key.Contains(req.Search));
            }
            int Count = query.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "Id Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var data = await query.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dtos = _mapper.Map<List<TypeHardwareConfigDto>>(data);
            return new BasePageResponse<List<TypeHardwareConfigDto>>
            {
                Success = true,
                Message = "获取数据成功",
                Count = Count,
                CurrentPage = req.PageNo,
                PageSize = req.PageSize,
                TotalPage = (int)Math.Ceiling((decimal)Count / req.PageSize),
                Data = dtos
            };
        }
    }
}
