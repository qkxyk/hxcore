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
    public class DeviceConfigService : IDeviceConfigService
    {
        private readonly ILogger<DeviceConfigService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceConfigRepository _dcr;

        public DeviceConfigService(ILogger<DeviceConfigService> log, IMapper mapper, IDeviceConfigRepository dcr)
        {
            this._log = log;
            this._mapper = mapper;
            this._dcr = dcr;
        }
        public Task<bool> IsExist(Expression<Func<DeviceConfigModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddDeviceConfigAsync(string account, DeviceConfigAddDto req, string deviceSn)
        {
            //检查是否存在相同的名称
            var data = await _dcr.Find(a => a.DeviceSn == deviceSn && a.DataName == req.DataName).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该设备已存在相同名称的配置数据" };
            }
            try
            {
                var entity = _mapper.Map<DeviceConfigModel>(req);
                entity.DeviceSn = deviceSn;
                entity.Create = account;
                await _dcr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的设备配置数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加设备配置数据失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateDeviceConfigAsync(string account, DeviceConfigUpdateDto req, string deviceSn)
        {
            //检查是否存在相同名称
            var d = await _dcr.FindAsync(req.Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备配置数据标示不存在" };
            }
            var data = await _dcr.Find(a => a.DeviceSn == deviceSn && a.DataName == req.DataName && a.Id != req.Id).CountAsync();
            if (data > 0)
            {
                return new BaseResponse { Success = false, Message = "该设备已存在相同名称的配置数据" };
            }
            try
            {
                var entity = _mapper.Map(req, d);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                entity.DeviceSn = deviceSn;
                await _dcr.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的设备配置数据成功");
                return new BaseResponse { Success = true, Message = "修改配置数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的设备配置数据失败,失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteDeviceConfigAsync(string account, int Id)
        {
            var data = await _dcr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备配置数据标示不存在" };
            }
            try
            {
                await _dcr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的设备配置数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的设备配置数据失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败" };
            }
        }
        public async Task<BaseResponse> GetDeviceConfigAsync(int Id)
        {
            var data = await _dcr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备配置数据标示不存在" };
            }
            var dto = _mapper.Map<DeviceConfigDto>(data);
            return new BResponse<DeviceConfigDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetDeviceConfigsAsync(string deviceSn)
        {
            var data = await _dcr.Find(a => a.DeviceSn == deviceSn).ToListAsync();
            var dtos = _mapper.Map<List<DeviceConfigDto>>(data);
            return new BResponse<List<DeviceConfigDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
