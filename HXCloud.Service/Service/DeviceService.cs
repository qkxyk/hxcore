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
        private readonly IDeviceRepository _device;
        private readonly IProjectService _ps;
        private readonly ITypeHardwareConfigService _th;

        public DeviceService(ILogger<DeviceService> log, IMapper mapper, IDeviceRepository device, IProjectService ps, ITypeHardwareConfigService th)
        {
            this._log = log;
            this._mapper = mapper;
            this._device = device;
            this._ps = ps;
            this._th = th;
        }
        public async Task<bool> IsExist(Expression<Func<DeviceModel, bool>> predicate)
        {
            var data = await _device.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                return false;
            }
            return true;
        }
        public async Task<DeviceCheckDto> IsExistCheck(Expression<Func<DeviceModel, bool>> predicate)
        {
            DeviceCheckDto dto = new DeviceCheckDto();
            var data = await _device.Find(predicate).FirstOrDefaultAsync();
            if (data == null)
            {
                dto.IsExist = false;
            }
            else
            {
                dto.IsExist = true;
                dto.GroupId = data.GroupId;
                dto.ProjectId = data.FullId;
            }
            return dto;
        }


        public async Task<BaseResponse> AddDeviceAsync(DeviceAddDto req, string account)
        {
            BaseResponse br = new BaseResponse();
            //查找是否存在相同的设备序号
            var device = _device.Find(a => a.DeviceNo == req.DeviceNo);
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
                await _device.AddAsync(entity, dtos);
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


    }
}
