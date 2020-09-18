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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WarnTypeController : ControllerBase
    {
        private readonly IWarnTypeService _wts;

        public WarnTypeController(IWarnTypeService wts)
        {
            this._wts = wts;
        }

        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddWarnType(WarnTypeAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _wts.AddWarnTypeAsync(Account, req);
            return rm;
        }
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateWarnType([FromBody]WarnTypeUpdateDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _wts.ModifyWarnTypeAsync(Account, req);
            return rm;
        }
        [TypeFilter(typeof(SuperAdminFilterAttribute))]
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteWarnType(int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _wts.DeleteWarnTypeAsync(Account, Id);
            return rm;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetWarnTypeById(int Id)
        {
            var rm = await _wts.FindWarnTypeByIdAsync(Id);
            return rm;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetWarnType()
        {
            var rm = await _wts.FindWarnTypeAsync();
            return rm;
        }
    }
}