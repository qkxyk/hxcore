using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeConfigController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITypeService _ts;
        private readonly ITypeConfigService _tc;

        public TypeConfigController(IConfiguration config, ITypeService ts, ITypeConfigService tc)
        {
            this._config = config;
            this._ts = ts;
            this._tc = tc;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Add(int typeId, TypeConfigAddViewModel req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tc.AddAsync(typeId, req, Account);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Update(int typeId, TypeConfigUpdateViewModel req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tc.UpdateAsync(req, Account);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Delete(int typeId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tc.DeleteAsync(Id, Account);
            return rm;
        }

        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Get(int typeId, int Id)
        {
            var rm = await _tc.FindById(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetByType(int typeId, [FromQuery]TypeConfigPageRequestViewModel req)
        {
            var rm = await _tc.FindByType(typeId, req);
            return rm;
        }
        [HttpPatch("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> PatchDeviceConfigAsync(int Id,int typeId, [FromBody] JsonPatchDocument<TypeConfigData> req)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _tc.GetTypeConfigByIdAsync(Id);
            req.ApplyTo(data, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var ret = await _tc.PatchTypeConfigAsync(account, data);
            return ret;
        }
    }
}