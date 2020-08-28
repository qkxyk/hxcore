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

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{warnTypeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class WarnCodeController : ControllerBase
    {
        private readonly IWarnCodeService _wcs;

        public WarnCodeController(IWarnCodeService wcs)
        {
            this._wcs = wcs;
        }

        [HttpPost]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddWarnCode(int warnTypeId, WarnCodeAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _wcs.AddWarnCodeAsync(Account, warnTypeId, req);
            return rm;
        }
        [HttpPut("{Code}")]
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateWarnCode(int warnTypeId, string Code, string Description)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _wcs.UpdateWarnCodeDescriptionAsync(Account, Code, Description);
            return rm;
        }
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        [HttpDelete("{Code}")]
        public async Task<ActionResult<BaseResponse>> DeleteWarnCode(int warnTypeId, string Code)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _wcs.DeleteWarnCodeAsync(Account, Code);
            return rm;
        }
        [HttpGet("/api/[controller]")]
        public async Task<ActionResult<BaseResponse>> GetWarnCode([FromQuery]WarnCodePageRequest req)
        {
            var rm = await _wcs.GetPageWarnCodeAsync(req);
            return rm;
        }
    }
}