using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
        private readonly ITypeRepository _tr;

        public DeviceService(ILogger<DeviceService> log, IMapper mapper, IDeviceRepository dr, IProjectService ps, ITypeHardwareConfigService th, ITypeRepository tr)
        {
            this._log = log;
            this._mapper = mapper;
            this._dr = dr;
            this._ps = ps;
            this._th = th;
            this._tr = tr;
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
                    //设备增加所属场站编号和名称
                    pathId = $"{p.PathId}/{p.Id}";
                    PathName = $"{p.PathName}/{p.Name}";
                }
                else
                {
                    br.Success = false;
                    br.Message = "输入的场站不存在";
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
                entity.GroupId = GroupId;
                //获取类型硬件配置数据
                var data = await _th.GetTypeHardwareConfigAsync(req.TypeId);
                var dtos = _mapper.Map<List<TypeHardwareConfigModel>, List<DeviceHardwareConfigModel>>(data);
                foreach (var item in dtos)
                {
                    item.Device = entity;
                    item.Create = account;
                }
                await _dr.AddAsync(entity, dtos);
                br = new BResponse<string> { Data = entity.DeviceSn, Success = true, Message = "添加设备成功" };
                _log.LogInformation($"{account}删除类型标示为{entity.DeviceSn}的设备数据成功");
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}添加设备失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                br.Success = false;
                br.Message = "添加设备失败，请联系管理员";
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
            var migration = new DeviceMigrationModel { Create = account, CreateTime = DateTime.Now, CurrentPId = projectId, PrePId = entity.ProjectId, DeviceSn = entity.DeviceSn, DeviceNo = entity.DeviceNo, GroupId = GroupId, TypeId = 1 };
            string message = "";
            try
            {
                entity.ProjectId = projectId;
                entity.Modify = account;
                entity.ModifyTime = DateTime.Now;
                if (projectId.HasValue && projectId.Value != 0)
                {
                    message = "迁移设备";
                    var p = await _ps.GetProjectAsync(projectId.Value);
                    if (p != null)
                    {
                        entity.FullId = p.PathId == null ? p.Id.ToString() : p.PathId;
                        entity.FullName = p.PathName == null ? p.Name : p.PathName;
                        entity.GroupId = GroupId;//超级管理员可以跨组织迁移设备
                    }
                    else
                    {
                        return new BaseResponse { Success = false, Message = "输入的项目不存在" };
                    }
                }
                else
                {
                    message = "设备移入回收站";
                    entity.FullId = null;
                    entity.FullName = null;
                }
                await _dr.SaveDeviceWithMigrationAsync(entity, migration);
                _log.LogInformation($"{account}{message}成功");
                return new BaseResponse { Success = true, Message = $"{message}成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"{account}{message}标识为{DeviceSn}的设备失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = $"{message}失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 获取无项目的设备
        /// </summary>
        /// <param name="GroupId">组织编号</param>
        /// <param name="req">分页参数</param>
        /// <returns>返回无项目设备列表</returns>
        public async Task<BaseResponse> GetNoProjectDeviceAsync(string GroupId, BasePageRequest req)
        {
            var device = _dr.FindWithOnlineAndImages(a => a.GroupId == GroupId && a.ProjectId == null);
            int count = device.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "DeviceSn Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await device.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<List<DeviceDataDto>>(list);
            var br = new BasePageResponse<List<DeviceDataDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dto;
            return br;
        }
        //获取项目或者场站设备
        public async Task<BaseResponse> GetProjectDeviceAsync(string GroupId, int projectId, bool isSite, BasePageRequest req)
        {
            var device = _dr.FindWithOnlineAndImages(a => a.GroupId == GroupId);
            if (isSite)
            {
                device = device.Where(a => a.ProjectId == projectId);
            }
            else
            {
                var sites = await _ps.GetProjectSitesIdAsync(projectId);
                device = device.Where(a => sites.Contains(a.ProjectId.Value));
            }
            if (!string.IsNullOrWhiteSpace( req.Search))
            {
                device = device.Where(a => a.DeviceName.Contains(req.Search));
            }
           
            int count = device.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "DeviceSn Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await device.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<List<DeviceDataDto>>(list);
            var br = new BasePageResponse<List<DeviceDataDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dto;
            return br;

        }
        public async Task<BaseResponse> GetMyDevice(string GroupId, string roles, bool isAdmin, BasePageRequest req)
        {
            var device = _dr.FindWithOnlineAndImages(a => a.GroupId == GroupId);
            var sites = await _ps.GetMySitesIdAsync(GroupId, roles, isAdmin);
            device = device.Where(a => sites.Contains(a.ProjectId.Value));
            if (req.Search != null && req.Search.Trim() != "")
            {
                device = device.Where(a => a.DeviceName.Contains(req.Search));
            }
            int count = device.Count();
            string OrderExpression = "";
            if (string.IsNullOrEmpty(req.OrderBy))
            {
                OrderExpression = "DeviceSn Asc";
            }
            else
            {
                OrderExpression = string.Format("{0} {1}", req.OrderBy, req.OrderType);
            }
            var list = await device.OrderBy(OrderExpression).Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToListAsync();
            var dto = _mapper.Map<List<DeviceDataDto>>(list);
            var br = new BasePageResponse<List<DeviceDataDto>>();
            br.Success = true;
            br.Message = "获取数据成功";
            br.PageSize = req.PageSize;
            br.CurrentPage = req.PageNo;
            br.Count = count;
            br.TotalPage = (int)Math.Ceiling((decimal)count / req.PageSize);
            br.Data = dto;
            return br;
        }

        /// <summary>
        /// 获取项目或者场站下的设备编号
        /// </summary>
        /// <param name="projectId">项目或者场站编号</param>
        /// <param name="isSite">是否是场站</param>
        /// <returns>返回设备序列号列表</returns>
        public async Task<List<string>> GetProjectDeviceSnAsync(int projectId, bool isSite)
        {
            List<string> devices;
            if (isSite)
            {
                devices = await _dr.Find(a => a.ProjectId == projectId).Select(a => a.DeviceSn).ToListAsync();
            }
            else
            {
                //获取项目下的场站列表
                var sites = await _ps.GetProjectSitesIdAsync(projectId);
                devices = await _dr.Find(a => sites.Contains(a.ProjectId.Value)).Select(a => a.DeviceSn).ToListAsync();
            }
            return devices;
        }
        /// <summary>
        /// 获取我的设备编号
        /// </summary>
        /// <param name="GroupId">组织编号</param>
        /// <param name="roles">我的角色</param>
        /// <param name="isAdmin">是否管理员</param>
        /// <returns>返回我的设备编号列表</returns>
        public async Task<List<string>> GetMyDeviceSnAsync(string GroupId, string roles, bool isAdmin)
        {
            var sites = await _ps.GetMySitesIdAsync(GroupId, roles, isAdmin);
            var devices = await _dr.Find(a => sites.Contains(a.ProjectId.Value)).Select(a => a.DeviceSn).ToListAsync();
            return devices;
        }

        public async Task<BaseResponse> GetAllDeviceAsync(List<int> Sites)
        {
            var device = await _dr.FindWithOnlineAndImages(a => Sites.Contains(a.ProjectId.Value)).ToListAsync();
            var dtos = _mapper.Map<List<DeviceDataDto>>(device);
            return new BResponse<List<DeviceDataDto>> { Success = true, Message = "获取数据成功", Data = dtos };
        }

        /// <summary>
        /// 获取场站列表下的所有设备编号
        /// </summary>
        /// <param name="sites">场站列表</param>
        /// <returns>返回设备编号列表</returns>
        public async Task<List<string>> GetDeviceSnBySitesAsync(List<int> sites)
        {
            var sn = await _dr.Find(a => sites.Contains(a.ProjectId.Value)).Select(a => a.DeviceSn).ToListAsync();
            return sn;
        }

        /// <summary>
        /// 获取设备的总揽数据
        /// </summary>
        /// <param name="sites">场站编号集合</param>
        /// <returns></returns>
        public async Task<BaseResponse> GetDeviceOverViewAsync(List<int> sites, string GroupId)
        {
            var device = await _dr.FindWithOnline(a => sites.Contains(a.ProjectId.Value)).ToListAsync();
            //获取类型的分类以及分类下的子类型标示
            var Types = await _tr.Find(a => a.GroupId == GroupId && a.Parent == null).ToListAsync();
            if (Types == null || Types.Count == 0)
            {
                return new BaseResponse { Success = false, Message = "该组织没有添加类型" };
            }
            DeviceOverViewDto dto = new DeviceOverViewDto();
            List<DeviceTypeInfo> list = new List<DeviceTypeInfo>();
            foreach (var item in Types)
            {
                DeviceTypeInfo dti = new DeviceTypeInfo { TypeId = item.Id, TypeName = item.TypeName, Icon = item.ICON };
                List<int> listType = await _tr.FindTypeChildAsync(item.Id);
                var online = device.Where(a => listType.Contains(a.TypeId) && (a.DeviceOnline != null && a.DeviceOnline.State == true)).Count();
                var num = device.Where(a => listType.Contains(a.TypeId)).Count();
                dti.OnlineNum = online;
                dti.Num = num;
                dto.Num += dti.Num;
                dto.OnlineNum += dti.OnlineNum;
                dto.TypeData.Add(dti);
            }
            return new BResponse<DeviceOverViewDto> { Success = true, Message = "获取数据成功", Data = dto };
        }

        /// <summary>
        /// 删除设备信息
        /// </summary>
        /// <param name="DeviceSn">设备编号</param>
        /// <param name="path">设备图片保存路径</param>
        /// <returns>返回删除设备是否成功</returns>
        public async Task<BaseResponse> DeleteDeviceAsync(string Account, string DeviceSn, string path)
        {
            bool bRet = false;//用来记录repository的返回是否成功
            try
            {
                bRet = await _dr.DeleteAsync(DeviceSn);
                if (!bRet)//设备不存在
                {
                    return new BaseResponse { Success = false, Message = "输入的设备不存在" };
                }
                //删除磁盘上的设备图片
                DirectoryInfo di = new DirectoryInfo(path);
                di.Delete(true);
                _log.LogInformation($"{Account}删除设备{DeviceSn}成功");
                return new BaseResponse { Success = true, Message = "删除设备成功" };
            }
            catch (Exception ex)
            {

                if (bRet == true)
                {
                    _log.LogError($"{Account}删除设备{DeviceSn}成功，设备图片删除失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                    return new BaseResponse { Success = true, Message = "设备删除成功" };
                }
                else
                {
                    _log.LogError($"{Account}删除设备{DeviceSn}失败，失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                    return new BaseResponse { Success = false, Message = "删除设备失败，请联系管理员" };
                }
            }
        }

        /// <summary>
        /// 部分更新设备数据
        /// </summary>
        /// <param name="Account">操作人</param>
        /// <param name="DeviceSn">设备编号</param>
        /// <param name="req">要修改的数据</param>
        /// <returns>返回修改设备是否成功</returns>
        public async Task<BaseResponse> PathUpdateDeviceAsync(string Account, string DeviceSn, DevicePatchDto req)
        {
            try
            {
                var Device = await _dr.FindAsync(DeviceSn);
                _mapper.Map(req, Device);
                await _dr.SaveAsync(Device);
                _log.LogInformation($"{Account}设备编号为{DeviceSn}修改数据成功");
                return new BaseResponse { Success = true, Message = "修改数据成功" };
            }
            catch (Exception ex)
            {
                _log.LogError($"修改数据失败，失败原因：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "修改数据失败，请联系管理员" };
            }
        }
        /// <summary>
        /// 转换设备数据为设备修改数据
        /// </summary>
        /// <param name="DeviceSn">设备序列号</param>
        /// <returns>返回设备修改数据</returns>
        public async Task<DevicePatchDto> GetDevicePathDtoAsync(string DeviceSn)
        {
            var Device = await _dr.FindAsync(DeviceSn);
            if (Device != null)
            {
                var dto = _mapper.Map<DevicePatchDto>(Device);
                return dto;
            }
            return null;
        }
    }
}
