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
    public class DeviceVideoService : IDeviceVideoService
    {
        private readonly ILogger<DeviceVideoService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceVideoRepository _dvr;

        public DeviceVideoService(ILogger<DeviceVideoService> log, IMapper mapper, IDeviceVideoRepository dvr)
        {
            this._log = log;
            this._mapper = mapper;
            this._dvr = dvr;
        }
        public Task<bool> IsExist(Expression<Func<DeviceVideoModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<BaseResponse> AddDeviceVideoAsync(string account, DeviceVideoAddDto req, string deviceSn)
        {
            var data = await _dvr.Find(a => a.DeviceSn == deviceSn && a.VideoName == req.VideoName).ToListAsync();
            if (data.Count > 0)
            {
                return new BaseResponse { Success = false, Message = "该设备已添加过相同名称的摄像头" };
            }
            try
            {
                var entity = _mapper.Map<DeviceVideoModel>(req);
                entity.DeviceSn = deviceSn;
                entity.Create = account;
                await _dvr.AddAsync(entity);
                _log.LogInformation($"{account}添加标示为{entity.Id}的摄像头成功");
                return new HandleResponse<int> { Success = true, Message = "添加数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加摄像头失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加摄像头失败，请联系管理员" };
                throw;
            }
        }
        public async Task<BaseResponse> UpdateDeviceVideoAsync(string account, DeviceVideoUpdateDto req, string deviceSn)
        {
            var d = await _dvr.FindAsync(req.Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的设备视频数据不存在" };
            }
            var data = await _dvr.Find(a => a.DeviceSn == deviceSn && a.VideoName == req.VideoName).FirstOrDefaultAsync();
            if (data != null && data.Id != req.Id)
            {
                return new BaseResponse { Success = false, Message = "该设备已添加过相同名称的摄像头" };
            }
            try
            {
                var entity = _mapper.Map(req, d);
                entity.DeviceSn = deviceSn;
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _dvr.AddAsync(entity);
                _log.LogInformation($"{account}修改标示为{entity.Id}的摄像头成功");
                return new HandleResponse<int> { Success = true, Message = "修改数据成功", Key = entity.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改摄像头数据失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改摄像头数据失败，请联系管理员" };
                throw;
            }
        }
        public async Task<BaseResponse> DeleteDeviceVideoAsync(string account, int Id)
        {
            var data = await _dvr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的视频编号不存在" };
            }
            try
            {
                await _dvr.RemoveAsync(data);
                _log.LogInformation($"{account}删除标示为{Id}的视频数据成功");
                return new BaseResponse { Success = true, Message = "删除数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除摄像头失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除摄像头失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetDeviceVideoAsync(int Id)
        {
            var data = await _dvr.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的视频编号不存在" };
            }
            var dto = _mapper.Map<DeviceVideoDto>(data);
            return new BResponse<DeviceVideoDto> { Success = true, Message = "获取摄像头数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetDeviceVideoesAsync(string deviceSn)
        {
            var data = await _dvr.Find(a => a.DeviceSn == deviceSn).ToListAsync();
            var dtos = _mapper.Map<List<DeviceVideoDto>>(data);
            return new BResponse<List<DeviceVideoDto>> { Success = true, Message = "获取设备摄像头数据成功", Data = dtos };
        }
    }
}
