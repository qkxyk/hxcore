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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 类型支持添加、编辑类型名称、删除类型、根据类型标示获取类型信息（附带类型子类型）、根据组织标示回去类型、导入合续类型(暂未开发2020-4-13)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeController : ControllerBase
    {
        private readonly ILogger<TypeController> _log;
        private readonly ITypeService _ts;
        private readonly IGroupService _gs;
        private readonly IConfiguration _config;

        public TypeController(ILogger<TypeController> log, IConfiguration config, ITypeService ts, IGroupService gs)
        {
            this._log = log;
            this._ts = ts;
            this._gs = gs;
            this._config = config;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddType(TypeAddViewModel req)
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //验证输入的groupid是否存在
            var ex = await _gs.IsExist(a => a.Id == req.GroupId);
            if (!ex)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
            //验证用户权限
            if (!(isAdmin && (GroupId == req.GroupId || Code == _config["Group"])))
            {
                return Unauthorized("没有权限");
            }
            var ret = await _ts.AddType(req, Account);
            return ret;
        }
        /// <summary>
        /// 类型复制，把一个叶子节点类型复制到另一个非叶子节点下
        /// 只支持源类型类型为叶子节点的类型，目标类型为非叶子节点的类型
        /// </summary>
        /// <param name="SourceId">源类型标示</param>
        /// <param name="DestId">目标类型标示</param>
        /// <returns>把一个类型复制到另一个类型下</returns>
        [HttpPost("{Copy}")]
        public async Task<ActionResult<BaseResponse>> CopyTo(int SourceId, int DestId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{Id}")]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> Update(int Id, [FromBody]TypeUpdateViewModel req)
        {
            //var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _ts.UpdateType(req, Account);
            return ret;
        }

        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> Delete(int Id)
        {
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var ret = await _ts.DeleteTypeAsync(Id, account);
            return ret;
        }

        [HttpGet("{Id}")]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> Get(int Id)
        {
            //此处待商榷（要现在用户只能访问自己所在组织的类型）
            var ret = await _ts.GetTypeAsync(Id);
            return ret;
        }

        [HttpGet]
        [TypeFilter(typeof(TypeViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetGroupType()
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var ret = await _ts.GetGroupTypeAsync(GroupId);
            return ret;
        }


    }
}