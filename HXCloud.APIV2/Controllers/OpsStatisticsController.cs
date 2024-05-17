using HXCloud.APIV2.MiddleWares;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OpsStatisticsController : ControllerBase
    {
        private readonly IUserService _user;
        private readonly IOpsStatisticsService _ops;
        private readonly IAuthorizationService _authorizationService;
        private readonly IModuleOperateService _moduleOperateService;
        private readonly IRoleModuleOperateService _roleModuleOperateService;
        private readonly IDeviceService _ds;
        private readonly IRoleProjectService _rps;
        private readonly IModuleService _moduleService;
        private readonly IRoleService _role;

        public OpsStatisticsController(IUserService user, IOpsStatisticsService ops, IAuthorizationService authorizationService, IModuleOperateService moduleOperateService,
            IRoleModuleOperateService roleModuleOperateService, IDeviceService ds, IRoleProjectService rps, IModuleService moduleService, IRoleService role)
        {
            this._user = user;
            this._ops = ops;
            this._authorizationService = authorizationService;
            this._moduleOperateService = moduleOperateService;
            this._roleModuleOperateService = roleModuleOperateService;
            this._ds = ds;
            this._rps = rps;
            this._moduleService = moduleService;
            this._role = role;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetOpsDataAsync([FromQuery] OpsStatisticsRequest req)
        {
            var ret = new BaseResponse();
            //用户可以看到自己填写的，上级用户可以看到下级用户的填写的
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            #region 验证用户权限
            if (IsAdmin)
            {
                //管理员可以查看所有
                ret = await _ops.GetOpsStatisticsAsync(req, true, null, null);
            }
            else
            {
                //验证是否有查看权限
                //1、查看用户在运维模块中的角色，没有分配查看的用户则获取自己接单的数据
                var moduleId = await _moduleService.GetModuleIdByCodeAsync("DeviceOps");//获取模块标识
                var role = await _role.GetRoles(a => a.ModuleId == moduleId);//获取模块角色
                CheckModuleRequirement mr = new CheckModuleRequirement(role);
                var t = await _authorizationService.AuthorizeAsync(User, null, mr);//返回用户在该模块的角色
                if (t.Succeeded)
                {
                    var moduleRoles = mr.ModuleRoles;
                    //检测用户角色时候有该模块的查看权限
                    var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "StatisticsRepair");
                    var RoleOps = await _roleModuleOperateService.GetModuleOperatesAsync(operateId);
                    var ro = RoleOps.FindAll(a => moduleRoles.Contains(a));
                    if (ro == null || ro.Count == 0)
                    {
                        ret = await _ops.GetOpsStatisticsAsync(req, false, Account, null);
                    }
                    else
                    {
                        //获取角色分配的项目，以及项目管理的设备
                        var projects = await _rps.GetRoleSitesAsync(ro);
                        //获取设备列表
                        var devices = await _ds.GetDeviceSnBySitesAsync(projects);
                        ret = await _ops.GetOpsStatisticsAsync(req, false, null, devices);
                    }
                }
                else
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                }
            }
            #endregion
            ////判断有没有查看权限，如果有查看权限
            //var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            ////运维人员
            //var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            //int category = 0;
            //int.TryParse(ops, out category);
            //var users = await _user.GetUserAndChildAsync(Account, isAdmin);
            //var ret = await _ops.GetOpsStatisticsAsync(users.Keys.ToList(), req);
            return ret;
        }
    }
}
