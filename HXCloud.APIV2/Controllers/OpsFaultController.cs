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
    public class OpsFaultController : ControllerBase
    {
        private readonly IOpsFaultService _opsFault;

        public OpsFaultController(IOpsFaultService opsFault)
        {
            this._opsFault = opsFault;
        }
        [HttpPost]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<BaseResponse>> AddOpsFaultAsync([FromBody]OpsFaultAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _opsFault.IsExist(a => a.Code == req.Code);
            if (data)
            {
                return new BaseResponse { Success = false, Message = "已添加过相同的故障代码" };
            }
            var ret = await _opsFault.AddOpsFaultAsync(Account, req);
            return ret;
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<BaseResponse>> DeleteOpsFaultAsync(string Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _opsFault.IsExist(a => a.Code == Id);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的故障数据不存在" };
            }
            var ret = await _opsFault.DeleteOpsFaultAsync(Account, Id);
            return ret;
        }
        [HttpPut]
        [Authorize(Policy ="Admin")]
        public async Task<ActionResult<BaseResponse>> EditOpsFaultAsync([FromBody] OpsFaultEditDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _opsFault.IsExist(a => a.Code == req.Code);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的故障数据不存在" };
            }
            var ret = await _opsFault.EditOpsFaultAsync(Account, req);
            return ret;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetOpsFaultByCode(string Id)
        {
            var data = await _opsFault.GetOpsFaultByCodeAsync(Id);
            return data;
        }
    }
}
