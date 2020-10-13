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
    public class TypeModuleFeedbackController : ControllerBase
    {
        private readonly ITypeDataDefineService _td;
        private readonly ITypeService _ts;
        private readonly ITypeModuleControlService _tmcs;
        private readonly ITypeModuleFeedbackService _tfs;

        public TypeModuleFeedbackController(ITypeDataDefineService td, ITypeService ts, ITypeModuleControlService tmcs, ITypeModuleFeedbackService tfs)
        {
            this._td = td;
            this._ts = ts;
            this._tmcs = tmcs;
            this._tfs = tfs;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeModuleFeedbackAsync(int typeId, TypeModuleFeedbackAddDto req)
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
            #region 检测相关的控制项是否存在
            var exist = await _tmcs.IsExist(a => a.Id == req.ModuleControlId);
            if (!exist)
            {
                return new BaseResponse { Success = false, Message = "输入的控制项不存在" };
            }
            #endregion
            var rm = await _tfs.AddTypeModuleFeedbaskAsync(Account, req);
            return rm;
        }

        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> UpdateTypeModuleFeedbackAsync(int typeId, [FromBody]TypeModuleFeedbackUpdateDto req)
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
            var rm = await _tfs.UpdateTypeModuleFeedbackAsync(Account, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> DeleteTypeModuleFeedbackAsync(int typeId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tfs.DeleteTypeModuleFeedbackAsync(Account, Id);
            return rm;
        }
        [HttpGet("{ControlId}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetTypeModuleFeedbackAsync(int typeId, int ControlId)
        {
            var rm = await _tfs.GetFeedbackByControlIdAsync(ControlId);
            return rm;
        }
    }
}