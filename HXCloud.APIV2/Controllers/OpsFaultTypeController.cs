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
    public class OpsFaultTypeController : ControllerBase
    {
        private readonly IOpsFaultTypeService _opsFaultType;

        public OpsFaultTypeController(IOpsFaultTypeService opsFaultType)
        {
            this._opsFaultType = opsFaultType;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BaseResponse>> AddOpsFaultTypeAsync([FromBody] OpsFaultTypeAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var isExist = await _opsFaultType.IsExist(a => a.FaultTypeName == req.FaultTypeName);
            if (isExist)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的故障类型" };
            }
            var ret = await _opsFaultType.AddOpsFaultTypeAsync(Account, req);
            return ret;
        }
        //[HttpPut("{Id}")]
        [HttpPut]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BaseResponse>> EditOpsFaultTypeAsync(/*int Id, */[FromBody] OpsFaultTypeEditDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _opsFaultType.IsExist(a => a.FaultTypeId == req.Id);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的故障类型不存在" };
            }
            var ret = await _opsFaultType.EditOpsFaultTypeAsync(Account, req);
            return ret;
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BaseResponse>> DeleteOpsFaultTypeAsync(int Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _opsFaultType.IsExist(a => a.FaultTypeId == Id);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的故障类型不存在" };
            }
            var ret = await _opsFaultType.DeleteOpsFaultTypeAsync(Account, Id);
            return ret;
        }
        ///获取故障类型
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetOpsFaultTypeAsync(int Id)
        {
            //验证是否存在，如果存在验证是否是顶级节点
            var ret = await _opsFaultType.IsExistAsync(a => a.FaultTypeId == Id);
            if (ret == -1)
            {
                return new BaseResponse { Success = false, Message = "输入的故障类型数据不存在" };
            }
            var data = await _opsFaultType.GetOpsFaultTypeByIdAsync(Id, ret);
            return data;
        }
        /// <summary>
        /// 获取全部故障类型数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetAllOpsTypeAsync()
        {
            var data = await _opsFaultType.GetAllOpsFaultTypeAsync();
            return data;
        }
        //[HttpGet("Child")]
        //public async Task<ActionResult<BaseResponse>> GetChildOpsTypeAsync(int Id)
        //{
        //    var data = await _opsFaultType.GetOpsFaultTypeByParentIdAsync(Id);
        //    return data;
        //}
    }
}
