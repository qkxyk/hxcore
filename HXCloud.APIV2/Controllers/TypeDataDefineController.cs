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
    public class TypeDataDefineController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITypeDataDefineService _td;
        private readonly ITypeService _ts;

        public TypeDataDefineController(IConfiguration config, ITypeDataDefineService td, ITypeService ts)
        {
            this._config = config;
            this._td = td;
            this._ts = ts;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeDataDefine(int typeId, TypeDataDefineAddViewModel req)
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
            var rm = await _td.AddTypeDataDefine(typeId, req, Account);
            return rm;
        }

        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> UpdateTypeDataDefine(int typeId, TypeDataDefineUpdateViewModel req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _ts.IsExist(a => a.Id == typeId, out GroupId);

            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _td.TypeDataDefineUpdate(req, Account);
            return rm;
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteTypeDataDefine(int typeId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _td.DeleteTypeDataDefine(Id, Account);
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
            //var ret = _ts.IsExist(a => a.Id == typeId, out GroupId);

            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _td.GetDataDefine(Id);
            return rm;
        }

        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Get(int typeId, [FromQuery]TypeDataDefinePageRequest req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //string GroupId;
            //var ret = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _td.GetTypeDataDefines(typeId,req);
            return rm;
        }

    }
}