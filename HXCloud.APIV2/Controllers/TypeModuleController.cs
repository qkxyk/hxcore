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
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeModuleController : ControllerBase
    {
        private readonly ITypeService _ts;
        private readonly ITypeModuleService _tms;

        public TypeModuleController(ITypeService ts, ITypeModuleService tms)
        {
            this._ts = ts;
            this._tms = tms;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeModuleAsync(int typeId, [FromBody]TypeModuleAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            #region 检测类型
            var type = await _ts.CheckTypeAsync(typeId);
            if (!type.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型编号不存在" };
            }
            if (type.Status == 0)
            {
                return new BaseResponse { Success = false, Message = "该类型为目录型节点，不允许添加数据" };
            }
            #endregion
            var rm = await _tms.AddTypeModuleAsync(Account, typeId, req);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> UpdateTypeModuleAsync(int typeId, [FromBody]TypeModuleUpdateDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            #region 检测类型
            var type = await _ts.CheckTypeAsync(typeId);
            if (!type.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型编号不存在" };
            }
            if (type.Status == 0)
            {
                return new BaseResponse { Success = false, Message = "该类型为目录型节点，不允许添加数据" };
            }
            #endregion
            var rm = await _tms.UpdateTypeModuleAsync(Account, typeId, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> DeleteTypeModuleAsync(int typeId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tms.DeleteTypeModuleAsync(Account, Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetByTypeId(int typeId)
        {
            var rm = await _tms.GetTypeModulesByTypeIdAsync(typeId);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetById(int typeId, int Id)
        {
            var rm = await _tms.GetTypeModuleByIdAsync(Id);
            return rm;
        }
    }
}