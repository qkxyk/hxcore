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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 1.添加角色、2.修改角色信息、3.获取单个角色信息、4.获取组织角色信息、5.删除角色信息
    /// </summary>
    [Route("api/Group/{GroupId}/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<UserController> _log;
        private readonly IUserService _us;
        private readonly IRoleService _rs;
        private readonly IConfiguration _config;

        public RoleController(ILogger<UserController> log, IUserService us, IRoleService rs, IConfiguration config/*, IWebHostEnvironment webHostEnvironment*/)
        {
            this._log = log;
            this._us = us;
            this._rs = rs;
            this._config = config;
        }

        [HttpPost]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddRole(string GroupId, [FromBody] RoleAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _rs.AddRoleAsync(req, Account, GroupId);
            return rm;
        }

        [HttpPut]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateRole(string GroupId, [FromBody]RoleUpdateDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _rs.UpdateRoleAsync(req, Account, GroupId);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteRole(string GroupId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _rs.DeleteRoleAsync(Id, Account);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetRoles(string GroupId, [FromQuery]BasePageRequest req)
        {
            var rm = await _rs.GetRoles(GroupId, req);
            return rm;
        }
    }
}