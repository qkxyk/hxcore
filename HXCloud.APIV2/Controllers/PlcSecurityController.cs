using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// plc鉴权
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PlcSecurityController : ControllerBase
    {
        private readonly IPlcSecurityService _plc;
        private readonly IRoleService _rs;

        public PlcSecurityController(IPlcSecurityService plc, IRoleService rs)
        {
            this._plc = plc;
            this._rs = rs;
        }

        /// <summary>
        /// 添加plc鉴权
        /// </summary>
        /// <param name="req">鉴权数据</param>
        /// <returns>返回鉴权码</returns>
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddPlcSecurityAsync([FromBody] PlcSecurityAddDto req)
        {
            //验证是否是合法用户，并且用户是否有plc权限
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            if (string.IsNullOrWhiteSpace(Roles))
            {
                return Unauthorized("用户没有权限");
            }
            var list = Array.ConvertAll<string, int>(Roles.Split(','), s => int.Parse(s));
            if (!isAdmin)//非管理员验证用户是否有plc权限
            {
                var ret = await _rs.IsExist(a => a.RoleName == "PLC秘钥生成" && list.Contains(a.Id));
                if (!ret)
                {
                    return Forbid("用户没有权限操作");
                }
            }
            var data = await _plc.AddPlcSecurityAsync(Account, req);
            return data;
        }
    }
}
