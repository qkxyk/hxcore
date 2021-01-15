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
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeClassController : ControllerBase
    {
        private readonly ITypeService _ts;
        private readonly ITypeClassService _tcs;

        public TypeClassController(ITypeService ts, ITypeClassService tcs)
        {
            this._ts = ts;
            this._tcs = tcs;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeClass(int typeId, [FromBody] TypeClassAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tcs.AddTypeClassAsync(Account, typeId, req);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> UpdateTypeClass(int typeId, [FromBody] TypeClassUpdateDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tcs.UpdateAsync(req, Account);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> DeleteTypeClass(int typeId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tcs.DeleteAsync(Id, Account);
            return rm;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetTypeClass(int typeId)
        {
            var rm = await _tcs.GetByTypeIdAsync(typeId);
            return rm;
        }
    }
}
