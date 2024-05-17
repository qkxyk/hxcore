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
    [Route("api/{CraftComponentCatalogId}/[controller]")]
    [ApiController]
    [Authorize]
    public class CraftElementController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;
        private readonly ICraftComponentCatalogService _craftComponentCatalog;
        private readonly IUserService _user;
        private readonly ICraftElementService _craftElement;

        public CraftElementController(IWebHostEnvironment webHostEnvironment, IConfiguration config
            , ICraftComponentCatalogService craftComponentCatalog, IUserService user, ICraftElementService craftElement)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._config = config;
            this._craftComponentCatalog = craftComponentCatalog;
            this._user = user;
            this._craftElement = craftElement;
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddCraftElementAsync(int CraftComponentCatalogId, [FromForm] CraftElementAddReqest req)
        {
            //普通用户只能上传个人组件
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var isAdmin = await _user.IsAdminAsync(userId);
            CraftElementAddDto dto = new CraftElementAddDto { Data = req.Data, Name = req.Name };
            var cata = await _craftComponentCatalog.IsExistWithElementAsync(a => a.Id == CraftComponentCatalogId);
            if (!cata.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件类型编号不存在" };
            }
            if (cata.CraftType != 1 && !isAdmin)
            {
                return new BaseResponse { Success = false, Message = "个人只能添加私人组件" };
            }
            dto.ElementType = cata.CraftType;

            //判断是否上传文件
            if (req.file != null)
            {
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
                if (length > 1024 * 1024 * 50) //50M
                {
                    return new BaseResponse { Success = false, Message = "上传的文件不能大于50M" };
                }
                //类型图片保存的相对路径：Image+组织编号+CraftComponent+图片名称
                string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
                                                                     //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
                string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//图片修改为时间加后缀名
                string userPath = Path.Combine(groupId, "CraftComponent", "Elements");//设备图片保存位置

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
                    dto.Image = path; 
                    var br = await _craftElement.AddCraftElementAsync(account, userId, CraftComponentCatalogId, dto);
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
            else
            {
                //CraftElementAddDto dto = new CraftElementAddDto { Data = req.Data, Name = req.Name };
                var br = await _craftElement.AddCraftElementAsync(account, userId, CraftComponentCatalogId, dto);
                return br;
            }
        }
        [HttpPut("{Id}")]
        public async Task<ActionResult<BaseResponse>> EditCraftElementAsync(int CraftComponentCatalogId, int Id, [FromForm] CraftElementEditRequest req)
        {
            //普通用户只能上传个人组件
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var isAdmin = await _user.IsAdminAsync(userId);
            //验证输入的工艺组件类型是否存在
            var exist = await _craftComponentCatalog.IsExist(a => a.Id == CraftComponentCatalogId);
            if (!exist)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件类型不存在" };
            }
            var elementExist = await _craftElement.IsExistAsync(a => a.Id == Id);
            if (elementExist == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件不存在" };
            }
            else
            {
                //判断是不是个人组件，并且个人组件是否和用户id相同
                if (elementExist.ElementType != 1)
                {
                    //公共组件
                    if (!isAdmin)
                    {
                        return new BaseResponse { Success = false, Message = "个人用户不能上传公共组件" };
                    }
                }
                else
                {
                    if (userId != elementExist.UserId)
                    {
                        return new BaseResponse { Success = false, Message = "无权修改其他人的个人组件" };
                    }
                }
            }
            if (req.file != null)//有上传文件
            {
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
                if (length > 1024 * 1024 * 50) //50M
                {
                    return new BaseResponse { Success = false, Message = "上传的文件不能大于5M" };
                }
                //类型图片保存的相对路径：Image+组织编号+CraftComponent+图片名称
                string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
                                                                     //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
                string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//图片修改为时间加后缀名
                string userPath = Path.Combine(groupId, "CraftComponent", "Elements");//设备图片保存位置
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
                    CraftElementEditDto dto = new CraftElementEditDto { Id = Id, CatalogId = CraftComponentCatalogId, Name = req.Name, Data = req.Data };
                    dto.Image = path;
                    dto.Id = Id;
                    var br = await _craftElement.EditCraftElementAsync(account, webRootPath, dto);
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
            }//end file
            else
            {
                CraftElementEditDto dto = new CraftElementEditDto { Id = Id, CatalogId = CraftComponentCatalogId, Name = req.Name, Data = req.Data };
                //dto.Icon = path;
                dto.Id = Id;
                var br = await _craftElement.EditCraftElementAsync(account, null, dto);
                return br;
            }
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteCraftElementAsync(int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var isAdmin = await _user.IsAdminAsync(userId);
            var elementExist = await _craftElement.IsExistAsync(a => a.Id == Id);
            if (elementExist == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件不存在" };
            }
            else
            {
                if (!isAdmin&&elementExist.UserId!=userId)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限删除该工艺组件" };
                }
            }
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _craftElement.DeleteCraftElementAsync(account, Id, webRootPath);
            return ret;
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetElementByIdAsync(int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var isAdmin = await _user.IsAdminAsync(userId);
            var elementExist = await _craftElement.IsExistAsync(a => a.Id == Id);
            if (elementExist == null)
            {
                return new BaseResponse { Success = false, Message = "输入的工艺组件不存在" };
            }
            else
            {
                if (!isAdmin && elementExist.UserId != userId)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限查看该工艺组件" };
                }
            }
            var ret = await _craftElement.GetCraftElementByIdAsync(Id);
            return ret;
        }
    }
}
