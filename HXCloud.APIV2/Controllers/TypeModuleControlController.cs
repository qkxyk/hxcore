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
using Org.BouncyCastle.Crypto.Prng.Drbg;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeModuleControlController : ControllerBase
    {
        private readonly ITypeDataDefineService _td;
        private readonly ITypeService _ts;
        private readonly ITypeModuleService _tms;
        private readonly ITypeModuleControlService _tmcs;

        public TypeModuleControlController(ITypeDataDefineService td, ITypeService ts, ITypeModuleService tms, ITypeModuleControlService tmcs)
        {
            this._td = td;
            this._ts = ts;
            this._tms = tms;
            this._tmcs = tmcs;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeModuleControlAsync(int typeId, [FromBody]TypeModuleControlAddDto req)
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
            #region 检查模块是否存在
            var exist = await _tms.IsExist(a => a.Id == req.ModuleId&&a.TypeId==typeId);
            if (!exist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型模块在该类型下不存在" };
            }
            #endregion
            var rm = await _tmcs.AddTypeModuleControlAsync(Account, req);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> UpdateTypeModuleControlAsync(int typeId, [FromBody]TypeModuleControlUpdateDto req)
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
            var control = await _tmcs.IsExistCheck(a => a.Id == req.Id);
            if (control.IsExist==false)
            {
                return new BaseResponse { Success = false, Message = "输入的控制数据编号不存在" };
            }
            #region 检查模块是否存在
            var exist = await _tms.IsExist(a => a.Id == control.ModuleId && a.TypeId == typeId);
            if (!exist)
            {
                return new BaseResponse { Success = false, Message = "输入的类型模块在该类型下不存在" };
            }
            #endregion
            #region 检测数据定义标示是否存在
            var td = await _td.IsExist(a => a.Id == req.DataDefineId);
            if (!td)
            {
                return new BaseResponse { Success = false, Message = "输入的类型数据定义不存在" };
            }
            #endregion
            var rm = await _tmcs.UpdateTypeModuleControlAsync(Account, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> DeleteTypeModuleControlAsync(int typeId, int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tmcs.DeleteTypeModuleControlAsync(Account, Id);
            return rm;
        }
        [HttpGet("Module/{ModuleId}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetByModuleIdAsync(int typeId, int ModuleId)
        {
            var exist = await _tms.IsExist(a => a.TypeId == typeId && a.Id == ModuleId);
            if (!exist)
            {
                return new BaseResponse { Success = false, Message = $"该类型下不存在编号为{ModuleId}的模块" };
            }
            var rm = await _tmcs.GetTypeModuleControlsByModuleIdAsync(ModuleId);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetByIdAsync(int typeId, int Id)
        {
            var control = await _tmcs.IsExistCheck(a => a.Id == Id);
            if (control.IsExist == false)
            {
                return new BaseResponse { Success = false, Message = "输入的控制数据编号不存在" };
            }
            #region 检查模块是否存在
            var exist = await _tms.IsExist(a => a.Id == control.ModuleId && a.TypeId == typeId);
            if (!exist)
            {
                return new BaseResponse { Success = false, Message =$"该类型下不存在控制数据编号为{Id}的模块" };
            }
            #endregion
            var rm = await _tmcs.GetTypeModuleControlsByIdAsync(Id);
            return rm;
        }
    }
}