using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> _log;
        private readonly IDeviceService _ds;
        private readonly IProjectService _ps;
        private readonly ITypeService _ts;
        private readonly IGroupService _gs;
        private readonly IConfiguration _config;
        private readonly IRoleProjectService _rp;

        //获取设备时附带设备的图片、设备工艺图等信息,设备列表只返回设备的基本信息

        //注入项目用来验证项目是否存在，角色项目验证是否有权限操作，类型用来验证类型信息，组织用来验证组织信息
        public DeviceController(ILogger<DeviceController> log, IDeviceService ds, IProjectService ps, ITypeService ts, IGroupService gs, IConfiguration config, IRoleProjectService rp)
        {
            this._log = log;
            this._ds = ds;
            this._ps = ps;
            this._ts = ts;
            this._gs = gs;
            this._config = config;
            this._rp = rp;
        }
        //添加设备需要导入类型的硬件配置数据
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddDevice(string GroupId, DeviceAddDto req)
        {
            BaseResponse br = new BaseResponse();
            bool bRet = await _gs.IsExist(opt => opt.Id == GroupId);
            if (!bRet)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在，请确认" };
            }
            var type = await _ts.CheckTypeAsync(req.TypeId);
            if (!type.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不存在，请确认" };
            }
            else if (type.Status == 0)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不能添加设备，请确认" };
            }
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;

            if (req.ProjectId.HasValue && req.ProjectId.Value != 0)
            {
                //string pathId, groupId;
                var pc = await _ps.GetProjectCheckAsync(req.ProjectId.Value);
                if (!pc.IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
                }
                if (!pc.IsSite)
                {
                    return new BaseResponse { Success = false, Message = "设备只能添加在场站下面" };
                }
                if (GroupId != pc.GroupId)
                {
                    return new BaseResponse { Success = false, Message = "输入的项目不存在或者项目与组织编号不匹配" };
                }

                if (isAdmin)
                {
                    if (GroupId != GId && Code != _config["Group"])
                    {
                        return new BaseResponse { Success = false, Message = "用户所属的组织编号和该组织编号不匹配" };
                    }
                }
                else
                {
                    bRet = await _rp.IsAuth(Roles, pc.PathId, 3);
                    if (!bRet)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有在该场站下添加设备的权限" };
                    }
                }
            }
            else//无项目的设备，只有管理员才有权限添加
            {
                if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限添加无项目的设备" };
                }
            }
            var rm = await _ds.AddDeviceAsync(req, Account, GroupId);
            return rm;
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateDevice(string GroupId, DeviceUpdateViewModel req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            //验证设备是否存在
            var dto = await _ds.IsExistCheck(a => a.DeviceSn == req.DeviceSn && a.GroupId == GroupId);
            if (!dto.IsExist)
            {
                return new BaseResponse { Success = false, Message = "该组织下不存在该设备" };
            }
            //验证权限
            #region
            if (dto.ProjectId.HasValue) //不是无项目的设备
            {
                if (!isAdmin)
                {
                    var bAuth = await _rp.IsAuth(Roles, dto.PathId, 2);//是否有权限编辑
                    if (!bAuth)
                    {
                        return Unauthorized("用户没有权限修改设备");
                    }
                }
                else
                {
                    if (GroupId != GId && Code != _config["Group"])
                    {
                        return Unauthorized("用户没有权限修改其它组织设备的权限");
                    }
                }

            }
            else
            {
                if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
                {
                    return Unauthorized("用户没有权限操作无项目的设备");
                }
            }
            #endregion
            var rm = await _ds.UpdateDeviceAsync(Account, GroupId, req);
            return rm;
        }
        [HttpPut("Type")]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceType(string GroupId, DeviceTypeUpdateDto req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            //验证设备是否存在
            var dto = await _ds.IsExistCheck(a => a.DeviceSn == req.DeviceSn && a.GroupId == GroupId);
            if (!dto.IsExist)
            {
                return new BaseResponse { Success = false, Message = "该组织下不存在该设备" };
            }
            //验证设备类型是否存在
            var type = await _ts.CheckTypeAsync(req.TypeId);
            if (!type.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不存在，请确认" };
            }
            else if (type.Status == 0)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不能添加设备，请确认" };
            }

            //验证权限
            #region
            if (dto.ProjectId.HasValue) //不是无项目的设备
            {
                if (!isAdmin)
                {
                    var bAuth = await _rp.IsAuth(Roles, dto.PathId, 2);//是否有权限编辑
                    if (!bAuth)
                    {
                        return Unauthorized("用户没有权限修改设备");
                    }
                }
                else
                {
                    if (GroupId != GId && Code != _config["Group"])
                    {
                        return Unauthorized("用户没有权限修改其它组织设备的权限");
                    }
                }

            }
            else
            {
                if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
                {
                    return Unauthorized("用户没有权限操作无项目的设备");
                }
            }
            #endregion
            var rm = await _ds.UpdateDeviceTypeAsync(Account, req.DeviceSn, req.TypeId);
            return rm;
        }

        /// <summary>
        /// 把设备放入回收站
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="DeviceSn"></param>
        /// <returns></returns>
        [HttpPut("Project")]
        public async Task<ActionResult<BaseResponse>> RemoveDeviceProject(string GroupId, [FromBody]DeviceDeleteDto req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            var dto = await _ds.IsExistCheck(a => a.DeviceSn == req.DeviceSn);
            if (!dto.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            //验证权限
            #region
            if (dto.ProjectId.HasValue) //不是无项目的设备
            {
                if (!isAdmin)
                {
                    var bAuth = await _rp.IsAuth(Roles, dto.PathId, 2);//是否有权限编辑
                    if (!bAuth)
                    {
                        return Unauthorized("用户没有权限修改设备");
                    }
                }
                else
                {
                    if (GroupId != GId && Code != _config["Group"])
                    {
                        return Unauthorized("用户没有权限修改其它组织设备的权限");
                    }
                }

            }
            else
            {
                //if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
                //{
                //    return Unauthorized("用户没有权限操作无项目的设备");
                //}
                return new BaseResponse { Success = false, Message = "该设备已在回收站中，请勿重复操作" };
            }
            #endregion
            var rm = await _ds.ChangeDeviceProject(Account, req.DeviceSn, GroupId, null);
            return rm;
        }

        [HttpPut("Migration")]
        public async Task<ActionResult<BaseResponse>> ChangeDeviceProject(string GroupId, string DeviceSn, int projectId)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            var dto = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!dto.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            if (dto.ProjectId.HasValue && dto.ProjectId.Value != 0)
            {
                return new BaseResponse { Success = false, Message = "只能迁移回收站中的设备" };
            }
            var pc = await _ps.GetProjectCheckAsync(projectId);
            if (!pc.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
            }
            if (!pc.IsSite)
            {
                return new BaseResponse { Success = false, Message = "设备只能添加在场站下面" };
            }
            //var p = await _ps.GetProjectAsync(projectId);
            //if (p == null)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的场站不存在" };
            //}
            ////只能迁移到场站
            //if (p.ProjectType == 0)
            //{
            //    return new BaseResponse { Success = false, Message = "设备只能迁移到场站" };
            //}
            //只有超级管理员有权限跨组织迁移
            if (pc.GroupId != GroupId)
            {
                if (isAdmin && Code == _config["Group"])
                {
                    GroupId = pc.GroupId;
                }
                else
                {
                    return Unauthorized("用户没有权限跨组织迁移设备");
                }
            }
            else
            {
                if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
                {
                    return Unauthorized("用户没有权限迁移设备");
                }
            }
            var rm = await _ds.ChangeDeviceProject(Account, DeviceSn, GroupId, projectId);
            return rm;
        }

        /// <summary>
        /// 彻底删除设备
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="DeviceSn"></param>
        /// <returns></returns>
        [HttpDelete]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDevice(string GroupId, string DeviceSn)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            var dto = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!dto.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在" };
            }
            if (dto.ProjectId.HasValue && dto.ProjectId.Value != 0)
            {
                return new BaseResponse { Success = false, Message = "只能删除回收站的设备" };
            }
            if (!(isAdmin && (GroupId == GId || Code == _config["Group"])))
            {
                return Unauthorized("用户没有权限删除设备");
            }
            throw new NotImplementedException();
            //var rm = await _ds.
        }

        /// <summary>
        /// 获取项目或者场站下的设备
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="projectId"></param>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpGet("Project")]
        public async Task<ActionResult<BaseResponse>> GetProjectDevices(string GroupId, int projectId, [FromQuery]BasePageRequest req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            var p = await _ps.GetProjectAsync(projectId);
            if (p == null)
            {
                return new BaseResponse { Success = false, Message = "输入的场站不存在" };
            }
            bool isSite = false;
            if (p.ProjectType == Model.ProjectType.Site)
            {
                isSite = true;
            }
            //验证权限
            if (GroupId != GId)
            {
                if (!(isAdmin && Code == _config["Group"]))
                {
                    return Unauthorized("用户没有权限");
                }
            }
            else
            {
                if (!isAdmin)
                {
                    var bAccess = await _rp.IsAuth(Roles, p.PathId, 0);
                    if (!bAccess)
                    {
                        return Unauthorized("用户没有权限查看");
                    }
                }
            }
            var rm = await _ds.GetProjectDeviceAsync(GroupId, projectId, isSite, req);
            return rm;
        }

        [HttpGet("NoProject")]
        public async Task<ActionResult<BaseResponse>> GetNoProjectDevice(string GroupId, [FromQuery]BasePageRequest req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            //验证权限,只有管理员才能查看无项目的设备
            if (!isAdmin)
            {
                return Unauthorized("只有管理员才有权限查看无项目的设备");
            }
            else
            {
                if (GroupId != GId && Code != _config["Group"])
                {
                    return Unauthorized("用户没有权限查看其他组织的设备");
                }
            }
            var rm = await _ds.GetNoProjectDeviceAsync(GroupId, req);
            return rm;
        }
        /// <summary>
        /// 获取我的设备
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("MyDevice")]
        public async Task<ActionResult<BaseResponse>> GetMyDevice(string GroupId, [FromQuery]BasePageRequest req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            if (GroupId != GId)
            {
                if (!(isAdmin && Code == _config["Group"]))//超级管理员可以查看任何组织的全部设备
                {
                    return Unauthorized("输入的组织编号不正确");
                }
            }
            var rm = await _ds.GetMyDevice(GroupId, Roles, isAdmin, req);
            return rm;
        }

        [HttpGet("CheckDeviceControl/{DeviceSn}")]
        public async Task<ActionResult<BaseResponse>> CheckDeviceControl(string GroupId, string DeviceSn)
        {
            //获取用户登录信息
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").ToString();
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //1、验证是否同组织管理员，2、验证角色权限，3、对比设备上的场站和项目
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备编号不存在" };
            }
            //用户所在的组和超级管理员可以查看
            if (GroupId != device.GroupId || (IsAdmin && Code != _config["Group"]))
            {
                return new BaseResponse { Success = false, Message = "用户没有权限" };
            }
            if (!IsAdmin)        //非管理员验证权限
            {
                //是否有设备的编辑权限
                bool bAuth = await _rp.IsAuth(Roles, device.PathId, 1);
                if (!bAuth)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限" };
                }
            }
            return new BaseResponse { Success = true, Message = "用户可以控制该设备" };
        }

        /// <summary>
        /// 获取所有设备，不分页
        /// </summary>
        /// <param name="GroupId">组织标示</param>
        /// <param name="projectId">项目或者场站标示</param>
        /// <returns></returns>
        [HttpGet("AllDevice")]
        public async Task<ActionResult<BaseResponse>> GetAllDevice(string GroupId, [FromQuery] int projectId)
        {

            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            List<int> sites;
            if (projectId == 0)   //获取所有设备
            {
                sites = await _ps.GetMySitesIdAsync(GroupId, Roles, isAdmin);//获取所有有权限的场站标示列表
            }
            else //获取项目设备
            {
                var p = await _ps.GetProjectCheckAsync(projectId);
                if (!p.IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的项目或者场站编号不存在" };
                }
                if (!isAdmin)
                {
                    bool isAuth = await _rp.IsAuth(Roles, p.PathId, 0);
                    if (!isAuth)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限访问该项目或者场站" };
                    }
                }
                if (p.IsSite)
                {
                    sites = new List<int> { projectId };
                }
                else
                {
                    sites = await _ps.GetProjectSitesIdAsync(projectId);
                }
            }
            var rm = await _ds.GetAllDeviceAsync(sites);
            return rm;
        }
        /// <summary>
        /// 获取设备的总揽数据
        /// </summary>
        /// <param name="GroupId">组织编号</param>
        /// <param name="ProjectId">项目或者场站标示</param>
        /// <returns>返回设备的总揽数据</returns>
        [HttpGet("OverView")]
        public async Task<ActionResult<BaseResponse>> GetDeviceOverViewAsync(string GroupId, int? ProjectId)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            //判断是否是同一个组织，是否有权限
            if (GId != GroupId)
            {
                //判断是否超级管理员
                if (!(isAdmin && Code == _config["Group"]))
                {
                    return Unauthorized("用户没有权限查看其他组织的设备");
                }
            }
            //判断对项目或者场站有权限
            List<int> sites = new List<int>();
            if (ProjectId.HasValue && ProjectId.Value != 0)
            {
                var project = await _ps.GetProjectCheckAsync(ProjectId.Value);
                if (!project.IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的项目或者场站编号不存在" };
                }
                var rp = await _rp.IsAuth(Roles, project.PathId, 0);
                if (!rp)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限查看该项目的设备" };
                }
                if (project.IsSite)
                {
                    sites.Add(ProjectId.Value);
                }
                else
                {
                    sites = await _ps.GetProjectSitesIdAsync(ProjectId.Value);
                }
            }
            else
            {
                //获取我的场站编号
                sites = await _ps.GetMySitesIdAsync(GroupId, Roles, isAdmin);
            }
            //获取所有设备编号
            var ret = await _ds.GetDeviceOverViewAsync(sites, GroupId);
            return ret;
        }
        [HttpGet("Region")]
        public async Task<ActionResult<BaseResponse>> GetRegionDeviceAsync()
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            throw new NotImplementedException();
        }
    }
}