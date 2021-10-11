using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DataDefineWarnCodeController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDataDefineWarnCodeService _dwcs;

        public DataDefineWarnCodeController(IConfiguration config,
            IDataDefineWarnCodeService dwcs)
        {
            this._config = config;
            this._dwcs = dwcs;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddDataDefineWarnCodeAsync([FromBody] DataDefineWarnCodeAddDto req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;

            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限添加数据定义库");
            }
            //检测key和code是否存在
            var check = await _dwcs.CheckDataDefineWarnCodeAsync(req);
            if (!check.IsExist)
            {
                return new BaseResponse { Success = false, Message = check.Message };
            }
            var ret = await _dwcs.AddDataDefineWarnCodeAsync(Account, req);
            return ret;
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteDataDefineWarnCodeAsync(int Id)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;

            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限添加数据定义库");
            }
            var ret = await _dwcs.RemoveDataDefineWarnCodeAsync(Account, Id);
            return ret;
        }


        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetDataDefineWarnCodesAsync([FromQuery] DataDefineWarnCodeRequest req)
        {
            //超级管理员有权限
            /*    var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
                var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
                string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
                string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;*/
            string[] keys = null, codes = null;
            BaseResponse br = null;
            if (req.Flag)
            {
                if (string.IsNullOrWhiteSpace(req.DataKeys))
                {
                    return new BaseResponse { Success = false, Message = "请输入要查询的数据定义Key" };
                }
                else
                {
                    keys = req.DataKeys.Split(',');
                }
                br = await _dwcs.GetDataDefineWarnCodesAsync(true, keys);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(req.Codes))
                {
                    return new BaseResponse { Success = false, Message = "请输入要查询的报警编码" };
                }
                else
                {
                    codes = req.Codes.Split(',');
                }
                br = await _dwcs.GetDataDefineWarnCodesAsync(false, codes);
            }
            return br;
        }
        [HttpGet("Pages")]
        public async Task<ActionResult<BaseResponse>> GetPageDataDefineWarnCodeAsync([FromQuery] DataDefineWarnCodePageRequest req)
        {
            var ret = await _dwcs.GetPageDataDefineWarnCodesAsync(req);
            return ret;
        }
    }
}
