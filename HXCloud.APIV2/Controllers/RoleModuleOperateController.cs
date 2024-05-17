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
    public class RoleModuleOperateController : ControllerBase
    {
        private readonly IRoleModuleOperateService _roleModuleOperateService;
        private readonly IRoleService _roleService;
        private readonly IModuleOperateService _moduleOperateService;

        public RoleModuleOperateController(IRoleModuleOperateService roleModuleOperateService,IRoleService roleService,IModuleOperateService moduleOperateService)
        {
            this._roleModuleOperateService = roleModuleOperateService;
            this._roleService = roleService;
            this._moduleOperateService = moduleOperateService;
        }

        [HttpPost]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<BaseResponse>> AddRoleModuleOperateAsync([FromBody]RoleModuleOperateAddDto req)
        {
        
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _roleModuleOperateService.AddRoleModuleOperateAsync(Account, req);
            return data;
        }
        [HttpDelete("{RoleId}/{OperateId}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BaseResponse>> DeleteRoleModuleOperateAsync(int RoleId,int OperateId)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _roleModuleOperateService.DeleteRoleModuleOperateAsync(Account, RoleId, OperateId);
            return ret;
        }

        [HttpGet("{RoleId}")]
        public async Task<ActionResult<BaseResponse>> GetRoleModuleOperateAsync(int RoleId)
        {
            var data = await _roleModuleOperateService.GetRoleOperatesAsync(RoleId);
            return data;
        }

    }
}
