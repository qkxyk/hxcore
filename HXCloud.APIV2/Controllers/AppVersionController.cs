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
    public class AppVersionController : ControllerBase
    {
        private readonly IAppVersionService _app;
        private readonly ILogger<AppVersionController> _log;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// 移动端更新包，上传更新包、编辑更新包、获取最新的更新包，获取分页的更新包
        /// </summary>
        public AppVersionController(IAppVersionService app, ILogger<AppVersionController> log, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            this._app = app;
            this._log = log;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddAppVersion(AppVersionAddDto req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限添加升级文件");
            }
            //文件后缀
            var fileExtension = Path.GetExtension(req.file.FileName);
            //暂时只支持apk文件
            const string fileFilt = ".apk";
            if (fileExtension == null)
            {
                return new BaseResponse { Success = false, Message = "上传的文件没有后缀" };
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "请上传后缀名为apk" + "的升级文件"
                };
            }
            //判断文件大小    
            long length = req.file.Length;
            if (length > 1024 * 1024 * 30) //30M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于30M" };
            }
            //类型图片保存的相对路径：Files+组织编号+TypeFiles+TypeId+文件名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//头像名称修改为用户编号加后缀名
            string userPath = Path.Combine(GroupId, "appFiles");//保存位置
            userPath = Path.Combine(_config["StoredFilesPath"], userPath);
            string path = Path.Combine(userPath, ext);//文件保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含文件名称
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, ext);//文件的物理路径
            try
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await req.file.CopyToAsync(stream);
                }
                var br = await _app.AddAppVersionAsync(Account, path, req);
                //br = await _us.UpdateUserImageAsync(um.Id, um.Account, path);
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
                _log.LogError($"{Account}上传升级文件失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传升级文件失败" };
            }
        }

        [HttpPut]
        public async Task<ActionResult<BaseResponse>> AppVersionUpdate(AppVersionUpdateDto req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限修改升级文件");
            }
            var rm = await _app.UpdateAppVersionAsync(Account, req);
            return rm;
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> AppVersionDelete(int Id)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限删除升级文件");
            }
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _app.DeleteAppVersionAsync(Account, Id, webRootPath);
            return ret;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetAppVersion(int Id)
        {
            //所有合法用户都可以查看
            var rm = await _app.GetAppVersionAsync(Id);
            return rm;
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetPageAppVersion([FromQuery]BasePageRequest req)
        {
            var rm = await _app.GetPageAppVersionAsync(req);
            return rm;
        }
    }
}