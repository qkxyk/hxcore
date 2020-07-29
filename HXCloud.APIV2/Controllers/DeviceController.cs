using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
            bRet = await _ts.IsExist(opt => opt.Id == req.TypeId);
            if (!bRet)
            {
                return new BaseResponse { Success = false, Message = "输入的类型不存在，请确认" };
            }
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;

            if (req.ProjectId.HasValue && req.ProjectId.Value != 0)
            {
                string pathId, groupId;
                bRet = _ps.IsExist(req.ProjectId.Value, out pathId, out groupId);
                if (!bRet || GroupId != groupId)
                {
                    return new BaseResponse { Success = false, Message = "输入的项目不存在或者项目与组织编号不匹配" };
                }
                if (pathId == null || "" == pathId)
                {
                    pathId = req.ProjectId.Value.ToString();
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
                    bRet = await _rp.IsAuth(Roles, pathId, 3);
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
        [HttpPut("{Type}")]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceType(string GroupId, string DeviceSn, int TypeId)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            //验证设备是否存在
            var dto = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn && a.GroupId == GroupId);
            if (!dto.IsExist)
            {
                return new BaseResponse { Success = false, Message = "该组织下不存在该设备" };
            }
            //验证设备类型是否存在
            var bType = await _ts.IsExist(a => a.Id == TypeId && a.GroupId == GroupId);
            if (!bType)
            {
                return new BaseResponse { Success = false, Message = "该组织下不存在此类型" };
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
            var rm = await _ds.UpdateDeviceTypeAsync(Account, DeviceSn, TypeId);
            return rm;
        }

        /// <summary>
        /// 把设备放入回收站
        /// </summary>
        /// <param name="GroupId"></param>
        /// <param name="DeviceSn"></param>
        /// <returns></returns>
        [HttpDelete("{Project}")]
        public async Task<ActionResult<BaseResponse>> RemoveDeviceProject(string GroupId, string DeviceSn)
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
        }

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

        }
    }
}