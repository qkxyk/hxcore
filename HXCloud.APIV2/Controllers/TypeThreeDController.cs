using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 类型的3d文件
    /// </summary>
    [Route("api/type/{typeId}/[controller]")]
    [ApiController]
    public class TypeThreeDController : ControllerBase
    {
        private readonly ILogger<TypeThreeDController> _log;
        private readonly ITypeGltfService _ti;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITypeService _ts;

        public TypeThreeDController(ILogger<TypeThreeDController> log, ITypeGltfService ti, IConfiguration config, IWebHostEnvironment webHostEnvironment, ITypeService ts)
        {
            this._log = log;
            this._ti = ti;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._ts = ts;
        }
        //只有管理员才能上传类型3D文件
        [HttpPost]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Add(int typeId, [FromForm] TypeGltfAddDto req)
        {
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //文件后缀
            var fileExtension = Path.GetExtension(req.file.FileName);
            //判断后缀是否是图片
            const string fileFilt = ".gltf|.fbx|.gltf|.glb|.FBX|.GLTF|.GLB|.json";
            if (fileExtension == null)
            {
                return new BaseResponse { Success = false, Message = "上传的文件没有后缀" };
            }
            if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
            {
                return new BaseResponse { Success = false, Message = "请上传gltf格式的文件" };
            }
            //判断文件大小    
            long length = req.file.Length;
            if (length > 1024 * 1024 * 200) //200M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于200M" };
            }
            //类型3d数据保存的相对路径：Files+组织编号+TypeGltf+类型3d数据名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            string ext = typeId.ToString() + fileExtension;//类型3d数据名称修改为用户编号加后缀名
            string userPath = Path.Combine(GroupId, "TypeGltf");//类型3d数据保存位置
            userPath = Path.Combine(_config["GltfPath"], userPath);
            string path = Path.Combine(userPath, ext);//类型3d数据保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含类型3d数据名称
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
                var br = await _ti.AddTypeGltfAsync(typeId, req, Account, path);
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
                _log.LogError($"{Account}上传类型3d数据失败,失败原因:{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传类型3d数据失败" };
            }
        }

        [HttpDelete()]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> Delete(int typeId)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //在service中处理删除图片，如果数据库删除成功，就标示为删除成功
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _ti.DeleteTypeGltfAsync(typeId,Account, webRootPath);
            return ret;
        }

        [HttpGet]
        [TypeFilter(typeof(TypeActionFilter))]
        public async Task<ActionResult<BaseResponse>> GetTypeImages(int typeId)
        {
            var ret = await _ti.GetTypeGltfAsync(typeId);
            return ret;
        }
    }
}
