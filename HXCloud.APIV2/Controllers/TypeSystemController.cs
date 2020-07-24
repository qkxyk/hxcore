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
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeSystemController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITypeService _ts;
        private readonly ITypeSystemService _tss;

        public TypeSystemController(IConfiguration config, ITypeService ts, ITypeSystemService tss)
        {
            this._config = config;
            this._ts = ts;
            this._tss = tss;
        }

        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeSystem(int typeId, TypeSystemAddDto req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //int status;
            //var ret = _ts.IsExist(typeId, out GroupId, out status);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (status == 0)
            //{
            //    return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tss.AddTypeSystemAsync(typeId, req, Account);
            return rm;
        }
        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> TypeSystemUpdate(int typeId, TypeSystemUpdateDto req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //bool bRet = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tss.UpdateTypeSystemAsync(typeId, req, Account);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> TypeSystemDelete(int typeId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //bool bRet = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tss.DeleteTypeSystemAsync(Id, Account);
            return rm;
        }

        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Get(int typeId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //bool bRet = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tss.GetSystemAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetTypeSystem(int typeId)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //bool bRet = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tss.GetTypeSystemAsync(typeId);
            return rm;
        }
    }
}