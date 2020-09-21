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
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeDisplayIconController : ControllerBase
    {
        private readonly ITypeDataDefineService _td;
        private readonly ITypeService _ts;
        private readonly ITypeDisplayIconService _tds;

        public TypeDisplayIconController(ITypeDataDefineService td, ITypeService ts, ITypeDisplayIconService tds)
        {
            this._td = td;
            this._ts = ts;
            this._tds = tds;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeDisplayIconAsync(int typeId, [FromBody]TypeDisplayIconAddDto req)
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
            #region 检测数据定义标示是否存在
            var td = await _td.IsExist(a => a.Id == req.DataDefineId);
            if (!td)
            {
                return new BaseResponse { Success = false, Message = "输入的类型数据定义不存在" };
            }
            #endregion
            var rm = await _tds.AddTypeDisplayIconAsync(Account, typeId, req);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> UpdateTypeDisplayIconAsync(int typeId, [FromBody] TypeDisplayIconUpdateDto req)
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
            #region 检测数据定义标示是否存在
            var td = await _td.IsExist(a => a.Id == req.DataDefineId);
            if (!td)
            {
                return new BaseResponse { Success = false, Message = "输入的类型数据定义不存在" };
            }
            #endregion
            var rm = await _tds.UpdateTypeDisplayIconAsync(Account, typeId, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> DeleteTypeDisplayIconAsync(int typeId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tds.DeleteDisplayIconAsync(Account, Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetTypeDisplayIconAsync(int typeId)
        {
            var rm = await _tds.GetTypeDisplayByTypeIdAsync(typeId);
            return rm;
        }
    }
}