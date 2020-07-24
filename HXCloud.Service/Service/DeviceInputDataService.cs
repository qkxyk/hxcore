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
    public class DeviceInputDataService : IDeviceInputDataService
    {
        private readonly ILogger<DeviceInputDataService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceInputDataRepository _dir;

        public DeviceInputDataService(ILogger<DeviceInputDataService> log, IMapper mapper, IDeviceInputDataRepository dir)
        {
            this._log = log;
            this._mapper = mapper;
            this._dir = dir;
        }
        public Task<bool> IsExist(Expression<Func<DeviceInputDataModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> AddDeviceInputDataAsync(string account, DeviceInputAddDto req, string deviceSn)
        {
            //一个设备只能添加一条
            var d = await _dir.Find(a => a.DeviceSn == deviceSn).CountAsync();
            if (d > 0)
            {
                return new BaseResponse { Success = false, Message = "该设备已存在有输入数据" };
            }
            try
            {
                var entity = _mapper.Map<DeviceInputDataModel>(req);
                entity.Create = account;
                entity.DeviceSn = deviceSn;
                await _dir.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的设备输入数据成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加设备输入数据失败，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> UpdateDeviceInputDataAsync(string account, DeviceInputDataUpdateDto req, string deviceSn)
        {
            var d = await _dir.FindAsync(req.Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备输入数据不存在" };
            }
            try
            {
                var entity = _mapper.Map(req, d);
                entity.Modify = account;
                entity.DeviceSn = deviceSn;
                entity.ModifyTime = DateTime.Now;
                await _dir.SaveAsync(entity);
                _log.LogInformation($"{account}修改标示为{req.Id}的设备输入数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标示为{req.Id}的设备输入数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteDeviceInputDataAsync(string account, int Id)
        {
            var d = await _dir.FindAsync(Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备输入数据不存在" };
            }
            try
            {
                await _dir.RemoveAsync(d);
                _log.LogInformation($"{account}删除标示为{Id}的设备输入数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除标示为{Id}的设备输入数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除数据失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> GetDeviceInputDataAsync(int Id)
        {
            var d = await _dir.FindAsync(Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备输入数据不存在" };
            }
            var dto = _mapper.Map<DeviceInputDto>(d);
            return new BResponse<DeviceInputDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetAllDeviceInputDataAsync(string deviceSn)
        {
            var d = await _dir.Find(a => a.DeviceSn == deviceSn).FirstOrDefaultAsync();
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "该设备没有添加输入数据" };
            }
            var dto = _mapper.Map<DeviceInputDto>(d);
            return new BResponse<DeviceInputDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
    }
}
