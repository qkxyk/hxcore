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

namespace HXCloud.APIV2.Controllers
{
    [Route("api/device/{deviceSn}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceImageController : ControllerBase
    {
        private readonly IDeviceImageService _dis;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;

        public DeviceImageController(IDeviceImageService dis, IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            this._dis = dis;
            this._webHostEnvironment = webHostEnvironment;
            this._config = config;
        }
        [HttpPost]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddDeviceImage(string deviceSn, [FromForm]DeviceImageAddDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            string groupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
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
            if (length > 1024 * 1024 * 5) //5M
            {
                return new BaseResponse { Success = false, Message = "上传的文件不能大于5M" };
            }
            //类型图片保存的相对路径：Image+组织编号+DeviceImage+DeviceSn+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            //string contentRootPath = _webHostEnvironment.ContentRootPath;//根目录
            string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + fileExtension;//图片修改为时间加后缀名
            string userPath = Path.Combine(groupId, "DeviceImage", deviceSn);//设备图片保存位置
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);
            string path = Path.Combine(userPath, ext);//图片保存地址（相对路径）
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含图片名称
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
                var br = await _dis.AddDeviceImageAsync(account, req, deviceSn, path);
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

        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateDeviceImage(string deviceSn, DeviceImageUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dis.UpdateDeviceImageAsync(account, req, deviceSn);
            return rm;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceImage(string deviceSn, int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //在service中处理删除图片，如果数据库删除成功，就标示为删除成功
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _dis.DeleteDeviceImageAsync(account, Id, webRootPath);
            return ret;
        }
        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceImage(string deviceSn, int Id)
        {
            var rm = await _dis.GetDeviceImageAsync(Id);
            return rm;
        }
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceImages(string deviceSn)
        {
            var rm = await _dis.GetAllDeviceImageAsync(deviceSn);
            return rm;
        }
    }
}