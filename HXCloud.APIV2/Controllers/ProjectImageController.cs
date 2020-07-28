using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using HXCloud.ViewModel;
using log4net.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/{ProjectId}/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectImageController : ControllerBase
    {
        private readonly ILogger<ProjectImageController> _log;
        private readonly IProjectImageService _pis;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProjectService _ps;
        private readonly IRoleProjectService _rps;

        public ProjectImageController(ILogger<ProjectImageController> log, IProjectImageService pis, IConfiguration config, IWebHostEnvironment webHostEnvironment, IProjectService ps, IRoleProjectService rps)
        {
            this._log = log;
            this._pis = pis;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._ps = ps;
            this._rps = rps;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddProjectImage(string GroupId, int projectId, [FromForm] ProjectImageAddDto req)
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

            //文件后缀
            var fileExtension = Path.GetExtension(req.file.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".gif|.jpg|.jpeg|.png";
            if (fileExtension == null)
            {
                return new BaseResponse { Success = false, Message = "上传的文件没有后缀" };
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                return new BaseResponse { Success = false, Message = "请上传jpg、png、gif格式的图片" };
            }
            //判断文件大小    
            long length = req.file.Length;
            if (length > 1024 * 1024 * 2) //2M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于2M" };
            }
            //类型图片保存的相对路径：Image+组织编号+TypeImage+TypeId+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//图片名称修改为日期加后缀名
            string userPath = Path.Combine(GroupId, "ProjectImage", projectId.ToString());//图片保存位置
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);
            string path = Path.Combine(userPath, ext);//头像保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含头像名称
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, ext);//头像的物理路径
            try
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await req.file.CopyToAsync(stream);
                }
                var br = await _pis.AddProjectImageAsync(req, path, Account);
                if (!br.Success)
                {
                    //删除已存在的logo
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                return br;
            }
            catch (Exception ex)
            {
                //删除已存在的logo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                _log.LogError($"{Account}上传项目图片失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传项目图片失败" };
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteProjectImage(string GroupId,int projectId,int Id)
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
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _pis.RemoveProjectImageAsync(Id, Account, webRootPath);
            return ret;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetImage(string GroupId,int projectId,int Id)
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
            var rm = await _pis.GetImageAsync(Id);
            return rm;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetProjectImage(string GroupId,int projectId)
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
            var rm = await _pis.GetProjectImageAsync(projectId);
            return rm;
        }

    }
}