using HXCloud.APIV2.Filters;
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
    public class BoxController : ControllerBase
    {
        private readonly IBoxService _box;

        public BoxController(IBoxService box)
        {
            this._box = box;
        }

        [HttpPost]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]

        public async Task<ActionResult<BaseResponse>> AddBoxAsync([FromBody] BoxAddDto req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _box.AddBoxAsync(account,req);
            return ret;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetBoxByIdAsync(int Id)
        {
            var ret = await _box.GetBoxAsync(Id);
            return ret;
        }
        [HttpGet]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetBoxAsync([FromQuery]BasePageRequest req)
        {
            var ret = await _box.GetPageBoxAsync(req);
            return ret;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteBoxAsync(int Id)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _box.DeleteBoxAsync(account, Id);
            return ret;
        }
        [AllowAnonymous]
        [HttpPost("Code")]
        public async Task<ActionResult<BaseResponse>> VerifyCodeAsync([FromBody]BoxVerifyReqiredDto req)
        {
            var ret = await _box.EncryptDataAsync(req.UUID, req.Serial, req.Imei);
            return ret;
        }
    }
}
