using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class DeviceHardwareConfigService : IDeviceHardwareConfigService
    {
        private readonly ILogger<DeviceHardwareConfigService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceHardwareConfigRepository _dhc;
        private readonly ITypeDataDefineRepository _tdr;

        public DeviceHardwareConfigService(ILogger<DeviceHardwareConfigService> log, IMapper mapper, IDeviceHardwareConfigRepository dhc, ITypeDataDefineRepository tdr)
        {
            this._log = log;
            this._mapper = mapper;
            this._dhc = dhc;
            this._tdr = tdr;
        }
        public Task<bool> IsExist(Expression<Func<DeviceHardwareConfigModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 添加设备PLC数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">PLC参数</param>
        /// <returns></returns>
        public async Task<BaseResponse> AddDeviceHardwareConfigAsync(string Account, string DeviceSn, DeviceHardwareConfigAddDto req)
        {
            var dataDefine = await _tdr.FindAsync(req.DataDefineId);
            if (dataDefine == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据定义编号不存在" };
            }
            var data = await _dhc.Find(a => a.DeviceSn == DeviceSn && a.Key == dataDefine.DataKey).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该设备下已存在相同的配置数据" };
            }
            try
            {
                var entity = _mapper.Map<DeviceHardwareConfigModel>(req);
                entity.Create = Account;
                entity.DeviceSn = DeviceSn;
                entity.Key = dataDefine.DataKey;
                entity.KeyName = dataDefine.DataName;
                entity.Unit = dataDefine.Unit;
                entity.Format = dataDefine.Format;
                await _dhc.AddAsync(entity);
                _log.LogInformation($"{Account}添加序列号为{DeviceSn}的设备标示为{entity.Id}的plc配置数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}添加设备序列为{DeviceSn}的plc配置数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败" };
            }
        }
        /// <summary>
        /// 修改设备plc配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备序列好</param>
        /// <param name="req">设备plc配置数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateDeviceHardwareConfigAsync(string Account, string DeviceSn, DeviceHardwareConfigUpdateDto req)
        {
            var entity = await _dhc.FindAsync(req.Id);
            if (entity == null)
            {
                return new BaseResponse { Success = false, Message = "输入的数据不存在" };
            }
            try
            {
                var dto = _mapper.Map(req, entity);
                dto.Modify = Account;
                dto.ModifyTime = DateTime.Now;
                await _dhc.SaveAsync(dto);
                _log.LogInformation($"{Account}修改标示为{req.Id}的设备PLC配置数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改备序列为{DeviceSn}的plc配置数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败" };
            }
        }

        /// <summary>
        /// 删除设备plc配置数据
        /// </summary>
        /// <param name="Id">设备PLC数据标示</param>
        /// <param name="account">操作人</param>
        /// <returns></returns>
        public async Task<BaseResponse> DeleteDeviceHardwareConfigAsync(int Id, string account)
        {
            var data = await _dhc.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备PLC配置数据不存在" };
            }
            try
            {
                await _dhc.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的设备PLC配置数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的设备PLC配置数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 获取设备PLC配置数据
        /// </summary>
        /// <param name="Id">设备PLC数据标示</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetHardwareConfigAsync(int Id)
        {
            var data = await _dhc.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备PLC配置数据不存在" };
            }
            var dto = _mapper.Map<DeviceHardwareConfigDto>(data);
            return new BResponse<DeviceHardwareConfigDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        /// <summary>
        /// 获取设备plc配置数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">分页参数</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetTypeHardwareConfigAsync(string DeviceSn, BasePageRequest req)
        {
            var query = _dhc.Find(a => a.DeviceSn == DeviceSn);
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
            var dtos = _mapper.Map<List<DeviceHardwareConfigDto>>(data);
            return new BasePageResponse<List<DeviceHardwareConfigDto>>
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
