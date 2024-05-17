using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CraftComponentCatalogController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly ICraftComponentCatalogService _craftComponentCatalog;
        private readonly IUserService _user;

        public CraftComponentCatalogController(IWebHostEnvironment webHostEnvironment, IConfiguration config
            , ICraftComponentCatalogService craftComponentCatalog, IUserService user)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._config = config;
            this._craftComponentCatalog = craftComponentCatalog;
            this._user = user;
        }
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BaseResponse>> AddCraftComponentCatalogAsync([FromForm] CraftComponentCatalogAddRequest req)
        {
            CraftComponentCatalogAddDto dto = new CraftComponentCatalogAddDto { Name = req.Name };
            if (req.ParentId.HasValue && req.ParentId.Value != 0)
            {
                var exist = await _craftComponentCatalog.IsExistWithElementAsync(a => a.Id == req.ParentId);
                if (!exist.IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的父工艺组件类型不存在" };
                }
                //父组件是否为叶子节点
                if (exist.IsLeaf)
                {
                    return new BaseResponse { Success = false, Message = "工艺组件类型叶子节点不能再添加节点" };
                }
                if (exist.CraftType!=req.CraftType)
                {
                    return new BaseResponse { Success = false, Message = "父组件类型和组件类型公共性必须一致" };
                }
            }
            dto.ParentId = req.ParentId;
            dto.CraftType = req.CraftType;
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //文件后缀
            var fileExtension = Path.GetExtension(req.file.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".gif|.jpg|.jpeg|.png|.icon";
            if (fileExtension == null)
            {
                return new BaseResponse { Success = false, Message = "上传的文件没有后缀" };
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                return new BaseResponse { Success = false, Message = "请上传jpg、png、gif、.icon格式的图片" };
            }
            //判断文件大小    
            long length = req.file.Length;
            if (length > 1024 * 1024 * 5) //5M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于5M" };
            }
            //类型图片保存的相对路径：Image+组织编号+CraftComponent+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//图片修改为时间加后缀名
            string userPath = Path.Combine(groupId, "CraftComponent");//设备图片保存位置
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);
            string path = Path.Combine(userPath, ext);//图片保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含图片名称
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, ext);//物理路径
            try
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await req.file.CopyToAsync(stream);
                }

                dto.Icon = path;
                var br = await _craftComponentCatalog.AddCraftComponentCatalogAsync(account, dto);
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
            catch/* (Exception ex)*/
            {
                //删除已存在的logo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return new BaseResponse { Success = false, Message = "上传类型图片失败" };
            }
        }
        [HttpPut("{Id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BaseResponse>> EditCraftComponentCatalogAsync(int Id, [FromForm] CraftComponentCatalogEditRequest req)
        {
            //验证输入的工艺组件类型是否存在
            var exist = await _craftComponentCatalog.IsExist(a => a.Id == Id);
            if (!exist)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件类型不存在" };
            }
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //文件后缀
            var fileExtension = Path.GetExtension(req.file.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".gif|.jpg|.jpeg|.png|.icon";
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
            if (length > 1024 * 1024 * 5) //5M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于5M" };
            }
            //类型图片保存的相对路径：Image+组织编号+CraftComponent+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//图片修改为时间加后缀名
            string userPath = Path.Combine(groupId, "CraftComponent");//设备图片保存位置
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);
            string path = Path.Combine(userPath, ext);//图片保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含图片名称
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, ext);//物理路径
            try
            {
                using (var stream = System.IO.File.Create(filePath))
                {
                    await req.file.CopyToAsync(stream);
                }
                CraftComponentCatalogEditDto dto = new CraftComponentCatalogEditDto { Name = req.Name };
                dto.Icon = path;
                dto.Id = Id;
                var br = await _craftComponentCatalog.EditCraftComponentCatalogAsync(account, webRootPath, dto);
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
            catch/* (Exception ex)*/
            {
                //删除已存在的logo
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                return new BaseResponse { Success = false, Message = "上传类型图片失败" };
            }
        }
        [HttpDelete("{Id}")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<BaseResponse>> DeleteCraftComponentCatalogAsync(int Id)
        {
            //验证输入的工艺组件类型是否存在

            var exist = await _craftComponentCatalog.IsExistWithElementAsync(a => a.Id == Id);
            if (!exist.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件类型不存在" };
            }
            if (exist.HasExistElement)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件类型存在关联的组件，请先删除组件" };
            }
            if (exist.HasChild)
            {
                return new BaseResponse { Success = false, Message = "该工艺组件下存在子节点，请先删除子节点" };
            }
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //在service中处理删除图片，如果数据库删除成功，就标示为删除成功
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _craftComponentCatalog.DeleteCraftComponentCatalogAsync(account, Id, webRootPath);
            return ret;
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetCraftComponentCatalogAsync(int Id)
        {
            var ret = await _craftComponentCatalog.GetCraftComponentCatalogAsync(Id);
            return ret;
        }
        //[HttpGet]
        //public async Task<ActionResult<BaseResponse>> GetAllCraftComponentCatalogAsync()
        //{
        //    //验证是否管理员，如果管理查看全部，非管理员查看公共的和自己的
        //    string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
        //    //获取用户角色
        //    var UserId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
        //    var Roles = await _userRole.GetUserRolesAsync(UserId);
        //    var data = await _craftComponentCatalog.GetAllCraftComponentCatalogAsync();
        //    return data;
        //}
        [HttpGet("My")]
        public async Task<ActionResult<BaseResponse>> GetMyCraftComponentCatalogAsync()
        {
            //验证是否管理员，如果管理查看全部，非管理员查看公共的和自己的
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //获取用户角色
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var isAdmin = await _user.IsAdminAsync(userId);
            //判断是否是管理员
            var data = await _craftComponentCatalog.GetMyCraftComponentCatalogAsync(userId, isAdmin);
            return data;
        }
    }
}
