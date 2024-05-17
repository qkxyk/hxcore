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
        private readonly IDeviceRepository _device;
        private readonly ITypeConfigRepository _typeConfig;

        public DeviceConfigService(ILogger<DeviceConfigService> log, IMapper mapper, IDeviceConfigRepository dcr,IDeviceRepository device,ITypeConfigRepository  typeConfig)
        {
            this._log = log;
            this._mapper = mapper;
            this._dcr = dcr;
            this._device = device;
            this._typeConfig = typeConfig;
        }
        public Task<bool> IsExist(Expression<Func<DeviceConfigModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddDeviceConfigAsync(string account, DeviceConfigAddDto req, string deviceSn)
        {
            req.Category = 1;
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
            req.Category = 1;

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
        /// <summary>
        /// 用于httppath部分修改数据
        /// </summary>
        /// <param name="Id">设备配置数据标识</param>
        /// <returns></returns>
        public async Task<DeviceConfigDto> GetDeviceConfigByIdAsync(int Id)
        {
            var data = await _dcr.FindAsync(Id);
            var dto = _mapper.Map<DeviceConfigDto>(data);
            return dto;
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
            var de = await _device.Find(a => a.DeviceSn == deviceSn).FirstOrDefaultAsync();
            if (de==null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备编号不存在" };
            }
            var typeCon = await _typeConfig.Find(a => a.TypeId == de.TypeId).ToListAsync();
            var data = await _dcr.Find(a => a.DeviceSn == deviceSn).ToListAsync();
            var dtos = _mapper.Map<List<DeviceConfigDto>>(data);
            //合并类型和设备的配置数据
            foreach (var item in typeCon)
            {
                dtos.Add(new DeviceConfigDto { Category = 0, DataName = item.DataName, DataType = item.DataType, DataValue = item.DataValue, Position = item.Position });
            }
            return new BResponse<List<DeviceConfigDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
        /// <summary>
        /// 修改设备配置数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="req">修改数据</param>
        /// <returns></returns>
        public async Task<BaseResponse> PatchDeviceConfigAsync(string Account, DeviceConfigDto req)
        {
            try
            {
                var data = await _dcr.FindAsync(req.Id);
                _mapper.Map(req, data);
                data.Modify = Account;
                data.ModifyTime = DateTime.Now;
                await _dcr.SaveAsync(data);
                _log.LogInformation($"{Account}修改标识为{req.Id}的设备配置数据成功");
                return new BResponse<int> { Success = true, Message = "修改数据成功", Data = req.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{Account}修改设备配置数据失败，失败原因{ex.Message}->{ex.InnerException}->{ex.StackTrace}");
                return new BaseResponse { Success = false, Message = "修改设备配置数据失败，请联系管理员" };
            }

        }
    }
}
