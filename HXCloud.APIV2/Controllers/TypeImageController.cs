using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
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
    //支持类型图片上传、删除和获取
    //类型只有叶子节点可以添加工艺图
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeImageController : ControllerBase
    {
        private readonly ILogger<TypeImageController> _log;
        private readonly ITypeImageService _ti;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITypeService _ts;

        public TypeImageController(ILogger<TypeImageController> log, ITypeImageService ti, IConfiguration config, IWebHostEnvironment webHostEnvironment, ITypeService ts)
        {
            this._log = log;
            this._ti = ti;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._ts = ts;
        }

        //只有管理员才能上传类型图片
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Add(int typeId, [FromForm]TypeImageAddViewModel req)
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
            //if (status == 0)
            //{
            //    return new BaseResponse { Success = false, Message = "目录节点类型不能添加具体数据" };
            //}
            ////var GroupId = await _ts.GetTypeGroupIdAsync(req.TypeId);
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //文件后缀
            var fileExtension = Path.GetExtension(req.file.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".gif|.jpg|.jpeg|.png|.mp4|.topo";
            if (fileExtension == null)
            {
                return new BaseResponse { Success = false, Message = "上传的文件没有后缀" };
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                return new BaseResponse { Success = false, Message = "请上传jpg、png、gif格式的图片或者topo文件" };
            }
            //判断文件大小    
            long length = req.file.Length;
            if (length > 1024 * 1024 * 2000) //20M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于20M" };
            }
            //类型图片保存的相对路径：Image+组织编号+TypeImage+TypeId+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//头像名称修改为用户编号加后缀名
            string userPath = Path.Combine(GroupId, "TypeImage", typeId.ToString());//用户头像保存位置
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
                var br = await _ti.AddTypeImage(typeId, req, Account, path);
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
                _log.LogError($"{Account}上传类型图片失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传类型图片失败" };
            }
        }

        [HttpDelete("{Id}")]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Delete(int typeId, int Id)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
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
            //在service中处理删除图片，如果数据库删除成功，就标示为删除成功
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _ti.DeleteTypeImage(Id, Account, webRootPath);
            return ret;
        }

        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetTypeImages(int typeId)
        {
            //string user = User.Identity.Name;
            //if (string.IsNullOrWhiteSpace(user))
            //{
            //    return Unauthorized("用户凭证缺失");
            //}
            //UserMessage um = JsonConvert.DeserializeObject<UserMessage>(user);
            //var GroupId = await _ts.GetTypeGroupIdAsync(typeId);
            //if (GroupId == null)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (um.GroupId != GroupId || !(um.IsAdmin && um.Code == _config["Group"]))
            //{
            //    return Unauthorized("用户没有权限查看");
            //}
            var ret = await _ti.GetTypeImage(typeId);
            return ret;
        }
    }
}