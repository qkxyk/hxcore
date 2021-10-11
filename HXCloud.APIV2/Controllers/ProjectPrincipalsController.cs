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
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/{ProjectId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectPrincipalsController : ControllerBase
    {
        private readonly ILogger<ProjectPrincipalsController> _log;
        private readonly IConfiguration _config;
        private readonly IProjectPrincipalsService _pps;
        private readonly IProjectService _ps;
        private readonly IRoleProjectService _rps;

        public ProjectPrincipalsController(ILogger<ProjectPrincipalsController> log, IConfiguration config, IProjectPrincipalsService pps, IProjectService ps, IRoleProjectService rps)
        {
            this._log = log;
            this._config = config;
            this._pps = pps;
            this._ps = ps;
            this._rps = rps;
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddProjectPrincipal(string GroupId, int projectId, ProjectPrincipalsAddDto req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            #region 验证用户权限
            var ret = await _ps.CheckProjectIdIsTopProjectAsync(projectId);
            if (!ret)
            {
                return new BaseResponse { Success = false, Message = "输入的项目编号不存在或者非顶级项目" };
            }
            var pathId = await _ps.GetPathId(projectId);
            if (pathId == null)
            {
                return new NotFoundResult();
            }
            else
            {
                pathId = $"{pathId}/{projectId}";
            }
            if (GId != GroupId)
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
                    var bAccess = await _rps.IsAuth(Roles, pathId, 2);
                    if (!bAccess)
                    {
                        return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                    }
                }
            }
            #endregion

            var br = await _pps.AddProjectPrincipalsAsync(Account, projectId, req);
            return br;

        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteProjectPrincipal(string GroupId, int projectId, int Id)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            #region 验证用户权限
            var pathId = await _ps.GetPathId(projectId);
            if (pathId == null)
            {
                return new NotFoundResult();
            }
            else
            {
                pathId = $"{pathId}/{projectId}";
            }
            if (GId != GroupId)
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
                    var bAccess = await _rps.IsAuth(Roles, pathId, 2);
                    if (!bAccess)
                    {
                        return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                    }
                }
            }
            #endregion
           var ret = await _pps.RemoveProjectPrincipalsAsync(Id, Account);
            return ret;
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult<BaseResponse>> UpdateProjectInfo(string GroupId, int projectId, int Id,ProjectPrincipalsUpdateDto req)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            var pathId = await _ps.GetPathId(projectId);
            if (pathId == null)
            {
                return new NotFoundResult();
            }
            else
            {
                pathId = $"{pathId}/{projectId}";
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
                    var bAccess = await _rps.IsAuth(Roles, pathId, 2);
                    if (!bAccess)
                    {
                        return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                    }
                }
            }
            var rm = await _pps.UpdateProjectPrincipalsAsync(Account,Id,req);
            return rm;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetProjectPrincipal(string GroupId, int projectId, int Id)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            #region 验证用户权限
            var pathId = await _ps.GetPathId(projectId);
            if (pathId == null)
            {
                return new NotFoundResult();
            }
            else
            {
                pathId = $"{pathId}/{projectId}";
            }
            if (GId != GroupId)
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
                    var bAccess = await _rps.IsAuth(Roles, pathId, 0);
                    if (!bAccess)
                    {
                        return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                    }
                }
            }
            #endregion
          
            var rm = await _pps.GetPrincipalAsync(Id);
            return rm;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetProjectPrincipals(string GroupId, int projectId)
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            #region 验证用户权限
            var pathId = await _ps.GetPathId(projectId);
            if (pathId == null)
            {
                return new NotFoundResult();
            }
            else
            {
                pathId = $"{pathId}/{projectId}";
            }
            if (GId != GroupId)
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
                    var bAccess = await _rps.IsAuth(Roles, pathId, 0);
                    if (!bAccess)
                    {
                        return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                    }
                }
            }
            #endregion
            var ret = await _ps.GetTopProjectIdAsync(projectId);
            if (ret == 0)
            {
                return new BaseResponse { Success = false, Message = "输入的项目编号不存在" };
            }
            var rm = await _pps.GetProjectPrincipalsAsync(ret);
            return rm;
        }

    }
}
