using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 1、获取用户角色.2、更新用户角色.3、删除用户单个角色
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRoleController : ControllerBase
    {
        private readonly ILogger<UserRoleController> _log;
        private readonly IUserRoleService _userRole;
        private readonly IConfiguration _config;
        private readonly IUserService _user;
        private readonly IRoleService _role;

        public UserRoleController(ILogger<UserRoleController> log, IUserRoleService userRole, IConfiguration config, IUserService user, IRoleService role)
        {
            this._log = log;
            this._userRole = userRole;
            this._config = config;
            this._user = user;
            this._role = role;
        }

        [HttpPost("{SaveUserRole}")]
        public async Task<ActionResult<BaseResponse>> AddUserRole(UserRoleAddViewModel req)
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;

            //1、判断用户和角色是否是同一个组织
            #region 优化
            /*
             //第一种验证
            if (!um.IsAdmin)
            {
                return Unauthorized("用户没有权限进行此操作");
            }
            else
            {
                //非同一个组织
                if (um.GroupId != req.GroupId)
                {
                    if (um.Code != _config["Group"])
                    {
                        return Unauthorized("用户没有权限进行此操作");
                    }
                }
            }
      //第二种验证
            if (um.IsAdmin && (um.GroupId == req.GroupId || um.Code == _config["Group"]))
            {
                //满足条件
            }
            else
            {
                return Unauthorized("用户没有权限进行此操作");
            }
                  */
            #endregion

            if (!(isAdmin && (GroupId == req.GroupId || Code == _config["Group"])))
            {
                return Unauthorized("用户没有权限进行此操作");
            }
            //验证用户和角色是否存在
            var b = await _user.IsExist(a => a.Id == req.UserId && a.GroupId == req.GroupId);
            if (!b)
            {
                return new BaseResponse() { Success = false, Message = $"该组织不存在Id为{req.UserId}的用户" };
            }
            var r = await _role.GetRoles(a => a.GroupId == req.GroupId && req.RoleId.Contains(a.Id));
            if (r.Count != req.RoleId.Count)
            {
                return new BaseResponse() { Success = false, Message = "包含有非法的角色标示" };
            }
            var ret = await _userRole.AddUserRoleAsync(req, Account);
            return ret;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetUserRole(int Id)
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            int UserId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            if (Id != UserId)
            {
                var groupId = await _user.GetUserGroupAsync(UserId);
                if (groupId == null)
                {
                    return new BaseResponse() { Success = false, Message = "该用户不存在" };
                }
                //管理员权限
                if (!(isAdmin && (GroupId == groupId || Code == _config["Group"])))
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "Text/plain", StatusCode = 401 };
                }
            }
            //用户自己和用户所在组管理员和超级管理员可以查看
            var ret = await _userRole.GetUserRole(UserId);
            return ret;
        }

    }
}