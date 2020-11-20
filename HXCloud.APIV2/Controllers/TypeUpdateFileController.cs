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
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeUpdateFileController : ControllerBase
    {
        private readonly ILogger<TypeUpdateFileController> _log;
        private readonly IConfiguration _config;
        private readonly ITypeUpdateFileService _tu;
        //private readonly ITypeService _ts;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TypeUpdateFileController(ILogger<TypeUpdateFileController> log, IConfiguration config, ITypeUpdateFileService tu,/* ITypeService ts,*/ IWebHostEnvironment webHostEnvironment)
        {
            this._log = log;
            this._config = config;
            this._tu = tu;
            //this._ts = ts;
            this._webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetTypeUpdateFiles(int typeId)
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
            var ret = await _tu.GetTypeUpdateFile(typeId);
            return ret;
        }

        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> AddTypeUpdateFile(int typeId, [FromForm]TypeUpdateFileAddViewModel req)
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
            //if (!ret)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //文件后缀
            var fileExtension = Path.GetExtension(req.file.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".bin";
            if (fileExtension == null)
            {
                return new BaseResponse { Success = false, Message = "上传的文件没有后缀" };
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                return new BaseResponse { Success = false, Message = "请上传后缀名为bin的升级文件" };
            }
            //判断文件大小    
            long length = req.file.Length;
            if (length > 1024 * 1024 * 5) //2M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于5M" };
            }
            //类型图片保存的相对路径：Files+组织编号+TypeFiles+TypeId+文件名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//头像名称修改为用户编号加后缀名
            string userPath = Path.Combine(GroupId, "TypeFiles", typeId.ToString());//用户头像保存位置
            userPath = Path.Combine(_config["StoredFilesPath"], userPath);
            string path = Path.Combine(userPath, ext);//文件保存地址（相对路径）
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
                var br = await _tu.AddTypeUpdateFile(typeId, req, Account, path);
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
                _log.LogError($"{Account}上传类型更新文件失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传类型更新文件失败，请联系管理员处理" };
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
            //var GroupId = await _tu.GetTypeGroupIdAsync(typeId);
            //if (GroupId == null)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的类型不存在" };
            //}
            //if (!(um.IsAdmin && (um.GroupId == GroupId || um.Code == _config["Group"])))
            //{
            //    return Unauthorized("用户没有权限");
            //}
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _tu.DeleteUpdateFile(Id, Account, webRootPath);
            return ret;
        }

    }
}