using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ModuleController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModuleController(IModuleService moduleService)
        {
            this._moduleService = moduleService;
        }

        [HttpGet("Id")]
        public async Task<BaseResponse> GetModuleByIdAsync(int Id)
        {
            var data = await _moduleService.GetModuleDataByIdAsync(Id);
            return data;
        }
        [HttpGet]
        public async Task<BaseResponse> GetModulesAsync()
        {
            var data = await _moduleService.GetModulesAsync();
            return data;
        }
        /// <summary>
        /// 添加模块，只有管理员有权限
        /// </summary>
        /// <param name="req">模块信息</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy ="Admin")]
        public async Task<BaseResponse> AddModuleAsync([FromBody]ModuleAddDto req)
        {
            //获取登录用户名
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _moduleService.AddModuleAsync(account, req);
            return data;
        }
        /// <summary>
        /// 修改模块，只有管理员有权限
        /// </summary>
        /// <param name="req">模块信息</param>
        /// <returns></returns>
        [HttpPut("Id")]
        [Authorize(Policy = "Admin")]
        public async Task<BaseResponse> UpdateModuleAsync(int Id, [FromBody] ModuleAddDto req)
        {
            //获取登录用户名
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _moduleService.UpdateModuleAsync(account,Id, req);
            return data;
        }
        /// <summary>
        /// 删除模块，只有管理员有权限
        /// </summary>
        /// <param name="req">模块信息</param>
        /// <returns></returns>
        [HttpDelete("Id")]
        [Authorize(Policy = "Admin")]
        public async Task<BaseResponse> DeleteModuleAsync(int Id)
        {
            //获取登录用户名
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _moduleService.DeleteModuleAsync(account, Id);
            return data;
        }
        [HttpPatch]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<BaseResponse>> PatchModuleAsync(int Id, [FromBody]JsonPatchDocument<ModuleDto> req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _moduleService.GetModuleByIdAsync(Id);
            req.ApplyTo(data, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ret = await _moduleService.PatchModuleAsync(account, data);
            return ret;
        }

    }
}
