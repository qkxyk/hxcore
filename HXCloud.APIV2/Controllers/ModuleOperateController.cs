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
    [Route("api/{ModuleId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ModuleOperateController : ControllerBase
    {
        private readonly IModuleOperateService _moduleOperate;

        public ModuleOperateController(IModuleOperateService moduleOperate)
        {
            this._moduleOperate = moduleOperate;
        }

        [HttpPost]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<BaseResponse>> AddModuleOperateAsync(int ModuleId,[FromBody]ModuleOperateAddDto req)
        {
            //if (ModuleId!=req.ModuleId)
            //{
            //    return BadRequest("输入的模块编号不一致");
            //}
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _moduleOperate.AddModuleOperateAsync(account, ModuleId,req);
            return ret;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetModuleOperateByIdAsync(int ModuleId,int Id)
        {
            var ret = await _moduleOperate.GetModuleOperateByIdAsync(Id);
            return ret;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetModuleOperatesAsync(int ModuleId)
        {
            var ret = await _moduleOperate.GetModuleOperatesAsync(ModuleId);
            return ret;
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<BaseResponse>> DeleteModuleOperateAsync(int ModuleId,int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _moduleOperate.DeleteModuleOperateByIdAsync(account, Id);
            return ret;
        }
    }
}
