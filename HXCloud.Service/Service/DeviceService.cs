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
    public class DeviceService : IDeviceService
    {
        private readonly ILogger<DeviceService> _log;
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _dr;
        private readonly IProjectService _ps;
        private readonly ITypeHardwareConfigService _th;

        public DeviceService(ILogger<DeviceService> log, IMapper mapper, IDeviceRepository dr, IProjectService ps, ITypeHardwareConfigService th)
        {
            this._log = log;
            this._mapper = mapper;
            this._dr = dr;
            this._ps = ps;
            this._th = th;
        }
        public async Task<bool> IsExist(Expression<Func<DeviceModel, bool>> predicate)
        {
            var data = await _dr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public async Task<DeviceCheckDto> IsExistCheck(Expression<Func<DeviceModel, bool>> predicate)
        {
            DeviceCheckDto dto = new DeviceCheckDto();
            var data = await _dr.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                dto.IsExist = false;
            }
            else
            {
                dto.IsExist = true;
                dto.GroupId = data.GroupId;
                dto.ProjectId = data.ProjectId;
                dto.PathId = data.FullId;
            }
            return dto;
        }


        public async Task<BaseResponse> AddDeviceAsync(DeviceAddDto req, string account, string GroupId)
        {
            BaseResponse br = new BaseResponse();
            //查找是否存在相同的设备序号
            var device = _dr.Find(a => a.DeviceNo == req.DeviceNo);
            if (await device.FirstOrDefaultAsync() != null)
            {
                br.Success = false;
                br.Message = "该设备已存在，请确认";
                return br;
            }
            //获取设备所属项目路径
            string pathId, PathName;
            if (req.ProjectId.HasValue && req.ProjectId.Value != 0)
            {
                var p = await _ps.GetProjectAsync(req.ProjectId.Value);
                if (p != null)
                {
                    pathId = p.PathId == null ? p.Id.ToString() : p.PathId;
                    PathName = p.PathName == null ? p.Name : p.PathName;
                }
                else
                {
                    br.Success = false;
                    br.Message = "输入的项目不存在";
                    return br;
                }
            }
            else
            {
                pathId = null;
                PathName = null;
            }

            try
            {
                var entity = _mapper.Map<DeviceModel>(req);
                entity.DeviceSn = Guid.NewGuid().ToString("N");
                entity.Create = account;
                entity.FullId = pathId;
                entity.FullName = PathName;
                //获取类型硬件配置数据
                var data = await _th.GetTypeHardwareConfigAsync(req.TypeId);
                var dtos = _mapper.Map<List<TypeHardwareConfigModel>, List<DeviceHardwareConfigModel>>(data);
                await _dr.AddAsync(entity, dtos);
                br = new BResponse<string> { Data = entity.DeviceSn, Success = true, Message = "添加设备成功" };
                _log.LogInformation($"{account}删除类型标示为{entity.DeviceSn}的设备数据成功");
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加设备失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                br.Success = false;
                br.Message = "添加设备失败";
            }
            return br;
        }

        public async Task<BaseResponse> UpdateDeviceAsync(string account, string GroupId, DeviceUpdateViewModel req)
        {
            var device = await _dr.FindAsync(req.DeviceSn);
            try
            {
                var dto = _mapper.Map(req, device);
                dto.Modify = account;
                dto.ModifyTime = DateTime.Now;
                await _dr.SaveAsync(dto);
                _log.LogInformation($"{account}修改标识为{req.DeviceSn}的设备成功");
                return new BaseResponse { Success = true, Message = "修改设备数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标识为{req.DeviceSn}的设备失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改设备数据失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> UpdateDeviceTypeAsync(string account, string DeviceSn, int TypeId)
        {
            var device = await _dr.FindAsync(DeviceSn);
            device.Modify = account;
            device.ModifyTime = DateTime.Now;
            device.TypeId = TypeId;
            //获取类型硬件配置数据
            var data = await _th.GetTypeHardwareConfigAsync(TypeId);
            var dtos = _mapper.Map<List<TypeHardwareConfigModel>, List<DeviceHardwareConfigModel>>(data);
            try
            {
                await _dr.SaveAsync(device, dtos);
                _log.LogInformation($"{account}修改标识为{DeviceSn}的设备类型成功");
                return new BaseResponse { Success = true, Message = "修改设备类型成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}修改标识为{DeviceSn}的设备类型失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改设备类型失败，请联系管理员" };
            }
        }

        public async Task<BaseResponse> ChangeDeviceProject(string account, string DeviceSn, string GroupId, int? projectId)
        {
            var entity = await _dr.FindAsync(DeviceSn);
            try
            {
                entity.ProjectId = projectId;
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                if (projectId.HasValue && projectId.Value != 0)
                {
                    var p = await _ps.GetProjectAsync(projectId.Value);
                    if (p != null)
                    {
                        entity.FullId = p.PathId == null ? p.Id.ToString() : p.PathId;
                        entity.FullName = p.PathName == null ? p.Name : p.PathName;
                    }
                    else
                    {
                        return new BaseResponse { Success = false, Message = "输入的项目不存在" };
                    }
                }
                else
                {
                    entity.FullId = null;
                    entity.FullName = null;
                }
                await _dr.SaveAsync(entity);
                _log.LogInformation($"{account}修改设备数据成功");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
