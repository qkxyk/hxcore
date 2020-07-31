using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Model;
using HXCloud.Repository;
using HXCloud.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;

namespace HXCloud.Service
{
    public class DeviceImageService : IDeviceImageService
    {
        private readonly ILogger<DeviceImageService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceImageRepository _dir;

        public DeviceImageService(ILogger<DeviceImageService> log, IMapper mapper, IDeviceImageRepository dir)
        {
            this._log = log;
            this._mapper = mapper;
            this._dir = dir;
        }
        public Task<bool> IsExist(Expression<Func<DeviceImageModel, bool>> predicate)
        {
            throw new NotImplementedException();
        }
        public async Task<BaseResponse> AddDeviceImageAsync(string account, DeviceImageAddDto req, string deviceSn, string path)
        {
            //验证图片名称是否重复
            var ret = await _dir.Find(a => a.DeviceSn == deviceSn && a.ImageName == req.ImageName).FirstOrDefaultAsync();
            if (ret != null)
            {
                return new BaseResponse { Success = false, Message = "该设备下已存在相同名称的图片" };
            }
            try
            {
                var dto = _mapper.Map<DeviceImageModel>(req);
                dto.DeviceSn = deviceSn;
                dto.url = path;
                dto.Create = account;
                await _dir.AddAsync(dto);
                _log.LogInformation($"{account}添加设备图片{dto.Id},名称为{dto.ImageName}成功");
                return new HandleResponse<int> { Success = true, Message = "添加设备图片成功", Key = dto.Id };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加设备图片失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "添加设备图片失败" };
            }
        }
        public async Task<BaseResponse> UpdateDeviceImageAsync(string account, DeviceImageUpdateDto req, string deviceSn)
        {
            var data = await _dir.Find(a => a.DeviceSn == deviceSn && a.ImageName == req.ImageName && a.Id != req.Id).FirstOrDefaultAsync();
            if (data != null)
            {
                return new BaseResponse { Success = false, Message = "该设备下已存在相同名称的图片" };
            }
            var d = await _dir.FindAsync(req.Id);
            if (d == null)
            {
                return new BaseResponse { Success = false, Message = "输入的图片不存在" };
            }
            try
            {
                var entity = _mapper.Map(req, d);
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                await _dir.SaveAsync(entity);
                _log.LogError($"{account}修改标示为{req.Id}的图片成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改设备图片信息失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改设备图片信息失败，请联系管理员" };
            }
        }
        public async Task<BaseResponse> DeleteDeviceImageAsync(string account, int Id, string path)
        {
            var ret = await _dir.FindAsync(Id);
            if (ret == null)
            {
                return new BaseResponse { Success = false, Message = "输入的图片不存在" };
            }
            try
            {
                //先删除文件
                string url = Path.Combine(path, ret.url);
                if (System.IO.File.Exists(url))
                {
                    System.IO.File.Delete(url);
                }
                await _dir.RemoveAsync(ret);
                _log.LogInformation($"{account}删除Id为｛Id｝图片成功");
                return new BaseResponse { Success = true, Message = "删除图片成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}删除Id为{Id}的图片失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "删除图片失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> GetDeviceImageAsync(int Id)
        {
            var data = await _dir.FindAsync(Id);
            if (data == null)
            {
                return new BaseResponse { Success = false, Message = "输入的图片不存在" };
            }
            var dto = _mapper.Map<DeviceImageDto>(data);
            return new BResponse<DeviceImageDto> { Success = true, Message = "获取数据成功", Data = dto };
        }
        public async Task<BaseResponse> GetAllDeviceImageAsync(string deviceSn)
        {
            var data = await _dir.Find(a => a.DeviceSn == deviceSn).ToListAsync();
            var dtos = _mapper.Map<List<DeviceImageDto>>(data);
            return new BResponse<List<DeviceImageDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }
    }
}
