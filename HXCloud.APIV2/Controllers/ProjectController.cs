using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.Service.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/Group/{GroupId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly ILogger<ProjectController> _log;
        private readonly IProjectService _ps;
        private readonly IConfiguration _config;
        private readonly IUserService _us;
        private readonly IRoleProjectService _rp;
        private readonly IGroupService _gs;

        public ProjectController(ILogger<ProjectController> log, IProjectService ps, IConfiguration config, IUserService us, IRoleProjectService rp, IGroupService gs)
        {
            this._log = log;
            this._ps = ps;
            this._config = config;
            this._us = us;
            this._rp = rp;
            this._gs = gs;
        }

        /// <summary>
        /// 根据标识获取项目或者场站信息,获取项目下所有的项目、场站信息
        /// </summary>
        /// <param name="Id">项目或者场站标识</param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetProject(int Id)
        {
            //本组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;


            string pathId;
            string groupId;
            //检查数据的项目或者场站编号是否存在
            bool bRet = _ps.IsExist(Id, out pathId, out groupId);
            if (!bRet)
            {
                return new BaseResponse { Success = false, Message = "输入的项目或者场站编号不存在" };
            }
            //检查是否有权限(获取项目的路径，如果是顶级项目，则传入当前项目编号)
            if (pathId == null)   //如果是顶级项目，则把当前Id赋值给项目路径
            {
                pathId = Id.ToString();
            }
            if (GroupId != groupId)
            {
                if (!(isAdmin && Code == _config["Group"]))//非超级管理员
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限" };
                }
            }
            else
            {
                if (!isAdmin) //非管理员
                {
                    bRet = await _rp.IsAuth(Roles, pathId, 0);
                    if (!bRet)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限" };
                    }
                }
            }
            var ret = await _ps.GetProject(Id);
            return ret;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Add(string GroupId, ProjectAddDto req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;

            string user = User.Identity.Name;
            if (string.IsNullOrWhiteSpace(user))
            {
                return Unauthorized("用户凭证缺失");
            }
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            var bRet = await _gs.IsExist(opt => opt.Id == req.GroupId);
            if (!bRet)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
            if (req.ProjectType == 1 && !req.ParentId.HasValue)
            {
                return new BaseResponse { Success = false, Message = "场站必须添加在项目下" };
            }
            //是否管理员
            if (isAdmin)
            {
                if (GroupId != req.GroupId && Code != _config["Group"])
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限添加此项目" };
                }
            }
            else
            {
                if (req.ParentId == null)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限添加顶级项目" };
                }
                else//非顶级项目
                {
                    string fullPath, groupId;
                    bRet = _ps.IsExist(req.ParentId.Value, out fullPath, out groupId);
                    if (!bRet)
                    {
                        return new BaseResponse { Success = false, Message = "父项目不存在" };
                    }
                    if (groupId != req.GroupId)
                    {
                        return new BaseResponse { Success = false, Message = "组织编号和项目非同一个组织" };
                    }
                    if (fullPath == null)
                    {
                        fullPath = req.ParentId.Value.ToString();
                    }
                    bRet = await _rp.IsAuth(Roles, fullPath, 3);
                    if (!bRet)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限添加此项目或者场站" };
                    }
                }
            }
            var ret = await _ps.AddProjectAsync(req, Account);
            return ret;
        }

        [HttpPut]
        public async Task<ActionResult<BaseResponse>> UpdateProjectInfo(string GroupId, ProjectUpdateDto req)
        {
            string GId = User.Claims.FirstOrDefault(a => a.Type == "Group").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            var pathId = await _ps.GetPathId(req.Id);
            if (pathId == null)
            {
                return new NotFoundResult();
            }
            if (GroupId != GId)
            {
                if (!(isAdmin && Code == _config["Group"]))
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                }
            }
            else
            {
                if (!isAdmin)
                {
                    var bAccess = await _rp.IsAuth(Roles, pathId, 2);
                    if (!bAccess)
                    {
                        return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                    }
                }
            }
            var rm = await _ps.UpdateProjectAsync(req, Account);
            return rm;
        }
    }

}