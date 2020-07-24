using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using HXCloud.APIV2.Filters;
using Microsoft.AspNetCore.Authorization;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 考虑类型模式数据量不大，不提供分页功能
    ///模式类型分为自动模式和自定义模式，自定义模式和硬件匹配，设置为4中自定义模式，目前自动模式下有嵌套子模式，自定义模式没有嵌套，
    /// </summary>
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeSchemaController : ControllerBase
    {
        private readonly ITypeSchemaService _tss;
        private readonly ITypeService _ts;
        private readonly IConfiguration _config;

        public TypeSchemaController(ITypeSchemaService tss, ITypeService ts, IConfiguration config)
        {
            this._tss = tss;
            this._ts = ts;
            this._config = config;
        }
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Add(int typeId, [FromBody]TypeSchemaAddViewModel req)
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
            var rm = await _tss.AddSchemaAsync(typeId, req, Account);
            return rm;
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Delete(int typeId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            ////检查是否存在此类型，模式在删除时验证是否存在
            //string GroupId;
            //var bRet = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型编号不存在" };
            //}

            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tss.Delete(Id, Account);
            return rm;
        }

        //修改模式名称
        [HttpPut]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Update(int typeId, [FromBody]TypeSchemaUpdateViewModel req)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            ////检查是否存在此模式
            //string GroupId;
            //var bRet = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型编号不存在" };
            //}

            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _tss.Update(req, Account);
            return rm;
        }
        /// <summary>
        /// serviceFilter+startup中使用依赖注入
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        //[ServiceFilter(typeof(TypeActionFilter))]
        //[TypeActionFilter(TypeId = {typeId})]
        //[TypeActionFilter(3)]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Get(int typeId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            ////检查是否存在此模式
            //string GroupId;
            //var bRet = _ts.IsExist(a => a.Id == typeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的模式编号不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tss.GetSchemaById(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetTypeSchema(int typeId)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            ////检查是否存在此模式
            //string GroupId;
            //var bRet = _ts.IsExist(a => a.Id == TypeId, out GroupId);
            //if (!bRet)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型编号不存在" };
            //}
            ////用户所在的组和超级管理员可以查看
            //if (um.GroupId != GroupId || (!um.IsAdmin && um.Code != _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var rm = await _tss.GetTypeSchema(typeId);
            return rm;
        }
    }
}