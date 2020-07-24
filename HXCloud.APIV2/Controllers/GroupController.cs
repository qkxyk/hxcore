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
using Newtonsoft.Json;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 组织要实现的功能：1.新增组织、2.组织修改、3.组织logo上传、4.获取本组织信息、5.获取组织列表（分页）
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<UserController> _log;
        private readonly IUserService _us;
        private readonly IGroupService _gs;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GroupController(ILogger<UserController> log, IUserService us, IGroupService gs, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            this._log = log;
            this._us = us;
            this._gs = gs;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> Add(GroupAddViewModel req)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && Code == _config["Group"]))
            {
                return Unauthorized("用户没有权限添加组织");
            }
            bool bGroup = await _gs.IsExist(a => a.GroupName == req.Name/*|| a.GroupCode == req.Code*/);
            if (bGroup)
            {
                return new BaseResponse { Success = false, Message = "已存在相同名称的组织" };
            }
            bGroup = await _gs.IsExist(a => a.GroupCode == req.Code);
            if (bGroup)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的组织代码" };
            }
            bool bUser = await _us.IsExist(a => a.Account == req.Account);
            if (bUser)
            {
                return new BaseResponse { Success = false, Message = "已存在相同的用户名" };
            }
            var rm = await _gs.AddGroupAsync(req, Account);
            return rm;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> Get(string Id)
        {
            //超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (GroupId != Id && !isAdmin && _config["Group"] != Code)//必须为超级管理员才能查看其他组织的信息
            {
                return Unauthorized("用户没有权限查看该组织信息");
            }
            var ret = await _gs.GetGroupAsync(Id);
            return ret;
        }
        /// <summary>
        /// 所有用户都有权限查看
        /// </summary>
        /// <param name="req"></param>
        /// <returns>只返回组织的标示、名称、logo</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse>> GetPageGroup([FromQuery]GroupPageListRequest req)
        {
            var ret = await _gs.GetPageGroupsAsync(req);
            return ret;
        }
        /// <summary>
        /// 修改组织信息，不允许修改组织代码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<BaseResponse>> Update(GroupUpdateViewModel req)
        {
            //本组织管理员或者超级管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!(isAdmin && (GroupId == req.GroupId || Code == _config["Group"])))
            {
                return Unauthorized("用户没有权限修改此组织信息");
            }

            var rm = await _gs.UpdateAsync(req, Account);
            return rm;
        }


        // [DisableRequestSizeLimit]
        [RequestSizeLimit(1024 * 1024 * 2)]
        [HttpPut("Logo")]
        public async Task<ActionResult<BaseResponse>> Logo([FromForm]IFormFile logo)
        {
            //本组织管理员有权限
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (!isAdmin)
            {
                return Unauthorized("用户没有权限上传组织logo");
            }
            BaseResponse br = new BaseResponse();
            if (logo == null)
            {
                br.Success = false;
                br.Message = "输入图像不能为空";
                return br;
            }
            //文件后缀
            var fileExtension = Path.GetExtension(logo.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".gif|.jpg|.jpeg|.png";
            if (fileExtension == null)
            {
                br.Success = false;
                br.Message = "上传的文件没有后缀";
                return br;
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                br.Success = false;
                br.Message = "请上传jpg、png、gif格式的图片";
                return br;
            }
            //判断文件大小    
            long length = logo.Length;
            if (length > 1024 * 1024 * 2) //2M
            {
                br.Success = false;
                br.Message = "上传的文件不能大于2M";
                return br;
            }
            if (!isAdmin)
            {
                return Unauthorized("用户没有权限上传组织logo");
            }
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = GroupId + fileExtension;//logo名称修改为组织编号加后缀名
            string userPath = GroupId;//组织logo保存在组织图片的根目录下
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
                    await logo.CopyToAsync(stream);
                }
                br = await _gs.UpdateLogoAsync(GroupId, path, Account);// _us.UpdateUserImageAsync(um.Id, um.Account, path);
                if (!br.Success)
                {
                    //删除已存在的logo
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                //删除已存在的logo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                br.Success = false;
                br.Message = "上传组织logo失败";
                _log.LogError($"{Account}上传组织logo失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
            }
            return br;
        }

    }
}