using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SimbossController : ControllerBase
    {
        private readonly ILogger<SimbossController> _log;
        private readonly IConfiguration _config;
        private readonly ISimbossService _simboss;

        public SimbossController(ILogger<SimbossController> log, IConfiguration config, ISimbossService simboss)
        {
            this._log = log;
            this._config = config;
            this._simboss = simboss;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddAsync([FromBody] SimbossAddDto req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限添加");
            }
            var rm = await _simboss.AddSimbossAsync(Account, req);
            return rm;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetAsync()
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限查看");
            }
            var rm = await _simboss.GetSimbossAsync();
            return rm;
        }
        //[HttpGet("Id")]
        //public async Task<ActionResult<BaseResponse>> GetAsync(int id)
        //{
        //    //超级管理员有权限
        //    var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
        //    var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
        //    string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
        //    string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
        //    if (!(isAdmin && Code == _config["Group"]))
        //    {
        //        return Unauthorized("用户没有权限查看");
        //    }
        //    return Task.FromResult<IActionResult>(Ok());
        //}

        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteAsync(int Id)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限删除");
            }
            var rm = await _simboss.DeleteSimbossAsync(Account, Id);
            return rm;
        }
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateAsync(SimbossUpdateDto req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限修改");
            }
            var rm = await _simboss.UpdateSimbossAsync(Account, req);
            return rm;
        }
    }
}
