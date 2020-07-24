using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _log;
        private readonly IMapper _mapper;
        private readonly IUserService _us;
        private readonly IGroupService _group;
        private readonly IDepartmentService _ds;
        private readonly IConfiguration _config;

        public DepartmentController(ILogger<DepartmentController> log, IMapper mapper, IUserService us, IGroupService group, IDepartmentService ds, IConfiguration config)
        {
            this._log = log;
            this._mapper = mapper;
            this._us = us;
            this._group = group;
            this._ds = ds;
            this._config = config;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Add(DepartmentAddViewModel req)
        {
            string user = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user))
            {
                return Unauthorized("用户凭证缺失");
            }
            UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //验证用户权限
            if (!(um.IsAdmin && (um.GroupId == req.GroupId || um.Code == _config["Group"])))
            {
                return Unauthorized("没有权限");
            }
            var ret = await _ds.AddDepartmentAsync(req, um.Account);
            return ret;
        }

        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Update(DepartmentUpdateViewModel req)
        {
            string user = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user))
            {
                return Unauthorized("用户凭证缺失");
            }
            var GroupId = await _ds.GetDepartmentGroupAsync(req.Id);
            if (GroupId == null)
            {
                return new BaseResponse { Success = false, Message = "输入的部门编号不存在" };
            }
            UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //验证用户权限
            if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            {
                return Unauthorized("没有权限");
            }
            var ret = await _ds.UpdateDepartmentAsync(req, GroupId, um.Account);
            return ret;
        }

        [HttpDelete]
        public async Task<ActionResult<BaseResponse>> Delete(int Id)
        {
            string user = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user))
            {
                return Unauthorized("用户凭证缺失");
            }
            var GroupId = await _ds.GetDepartmentGroupAsync(Id);
            if (GroupId == null)
            {
                return new BaseResponse { Success = false, Message = "输入的部门编号不存在" };
            }
            UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //验证用户权限
            if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            {
                return Unauthorized("没有权限");
            }
            var ret = await _ds.DeleteDepartmentAsync(Id, um.Account);
            return ret;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> Get(int Id)
        {
            string user = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user))
            {
                return Unauthorized("用户凭证缺失");
            }
            var GroupId = await _ds.GetDepartmentGroupAsync(Id);
            if (GroupId == null)
            {
                return new BaseResponse { Success = false, Message = "输入的部门编号不存在" };
            }
            UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //验证用户权限
            if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            {
                return Unauthorized("没有权限");
            }
            var ret = await _ds.GetDepartment(Id);
            return ret;
        }
        [HttpGet("Group")]
        public async Task<ActionResult<BaseResponse>> GetGroupDepartment(string GroupId)
        {
            string user = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user))
            {
                return Unauthorized("用户凭证缺失");
            }
            UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            if (string.IsNullOrWhiteSpace(GroupId))//如果没有输入组织编号则为查看本组织的信息
            {
                GroupId = um.GroupId;
            }
            else//检查组织编号是否存在
            {
                var g = await _group.IsExist(a => a.Id == GroupId);
                if (!g)
                {
                    return new BaseResponse { Success = false, Message = "输入的部门编号不存在" };
                }
            }
            //验证用户权限
            if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            {
                return Unauthorized("没有权限");
            }

            var ret = await _ds.GetGroupDepartment(GroupId);
            return ret;
        }
    }
}