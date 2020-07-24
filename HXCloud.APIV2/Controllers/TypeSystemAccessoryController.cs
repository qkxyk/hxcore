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
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/typesystem/{systemId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeSystemAccessoryController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITypeSystemService _tss;
        private readonly ITypeSystemAccessoryService _tsas;

        public TypeSystemAccessoryController(IConfiguration config, ITypeSystemService tss, ITypeSystemAccessoryService tsas)
        {
            this._config = config;
            this._tss = tss;
            this._tsas = tsas;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeSystemFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> Add(int systemId, TypeSystemAccessoryAddDto req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tss.IsExist(a => a.Id == systemId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tsas.AddSystemAccessoryAsync(systemId, req, Account);
            return rm;
        }

        [HttpPut]
        [TypeFilter(typeof(TypeSystemFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> TypeSystemAccessoryUpdate(int systemId, TypeSystemAccessoryUpdateDto req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tss.IsExist(a => a.Id == systemId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tsas.UpdateTypeSystemAccessoryAsync(systemId, req, Account);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeSystemFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> SystemAccessoryDelete(int systemId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tss.IsExist(a => a.Id == systemId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tsas.DeleteSystemAccessoryAsync(Id, Account);
            return rm;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeSystemFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetAccessory(int systemId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tss.IsExist(a => a.Id == systemId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tsas.GetAccessoryAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeSystemFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetSystemAccessory(int systemId, [FromQuery]BasePageRequest req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _tss.IsExist(a => a.Id == systemId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型子系统不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tsas.GetSystemAccessory(systemId, req);
            return rm;
        }
    }
}