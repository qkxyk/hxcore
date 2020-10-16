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

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 角色分配项目权限
    /// </summary>
    [Route("api/{GroupId}/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleProjectController : ControllerBase
    {
        private readonly IRoleService _rs;
        private readonly IProjectService _ps;
        private readonly IRoleProjectService _rps;
        private readonly IConfiguration _config;

        /// <summary>
        /// 角色的项目权限
        /// </summary>
        public RoleProjectController(IRoleService rs, IProjectService ps, IRoleProjectService rps, IConfiguration config)
        {
            this._rs = rs;
            this._ps = ps;
            this._rps = rps;
            this._config = config;
        }
        /// <summary>
        /// 添加或者修改角色项目
        /// </summary>
        /// <param name="GroupId">组织标示</param>
        /// <param name="req">角色和项目信息</param>
        /// <returns>返回是否成功</returns>
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddOrUpdateAsync(string GroupId, [FromBody]RoleProjectAddDto req)
        {
            //检测是否同一个组织，是否角色一致
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //只有管理员有权限操作
            if (!isAdmin)
            {
                return Unauthorized("用户没有权限获取角色项目");
            }
            else
            {
                if (GId != GroupId && Code != _config["Group"])
                {
                    return Unauthorized("用户没有权限获取角色权限");
                }
            }
            int[] projects = Array.ConvertAll<string, int>(req.ProjectId.Split(','), a => int.Parse(a));
            int[] types = Array.ConvertAll<string, int>(req.Operate.Split(','), a => int.Parse(a));
            if (projects.Length != types.Length)
            {
                return new BaseResponse { Success = false, Message = "项目编号和操作编号不匹配" };
            }
            var rm = await _rps.AddOrUpdateRoleProjectAsync(Account, req.RoleId, projects, types);
            return rm;
        }

        /// <summary>
        /// 获取角色项目
        /// </summary>
        /// <param name="GroupId">组织标示</param>
        /// <param name="roleId">角色标示</param>
        /// <returns>返回角色的项目列表</returns>
        [HttpGet("{roleId}")]
        public async Task<ActionResult<BaseResponse>> GetRoleProjectAsync(string GroupId, int roleId)
        {
            //检测是否同一个组织，是否角色一致
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //只有管理员有权限操作
            if (!isAdmin)
            {
                return Unauthorized("用户没有权限获取角色项目");
            }
            else
            {
                if (GId != GroupId && Code != _config["Group"])
                {
                    return Unauthorized("用户没有权限获取角色权限");
                }
            }

            var rd = await _rs.IsExist(a => a.Id == roleId);
            if (rd == false)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
            var rm = await _rps.GetRoleProjectAsync(roleId);
            return rm;
        }
    }
}