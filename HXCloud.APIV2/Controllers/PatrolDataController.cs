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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    /// <summary>
    /// 巡检数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatrolDataController : ControllerBase
    {
        private readonly ILogger<PatrolDataController> _logger;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDeviceService _ds;
        private readonly IUserService _user;
        private readonly IPatrolDataService _patrolData;
        private readonly IRoleProjectService _rps;

        public PatrolDataController(ILogger<PatrolDataController> logger, IConfiguration config, IWebHostEnvironment webHostEnvironment, IDeviceService ds, IUserService user, IPatrolDataService patrolData, IRoleProjectService rps)
        {
            this._logger = logger;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._ds = ds;
            this._user = user;
            this._patrolData = patrolData;
            this._rps = rps;
        }

        [HttpPost]
        [TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<BaseResponse> AddPatrolDataAsync([FromBody] PatrolDataAddRequest req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            int Id = 0;
            int.TryParse(User.Claims.FirstOrDefault(a => a.Type == "Id").Value, out Id);

            //检测关联的设备是否存在
            var data = await _ds.CheckDeviceAsync(req.DeviceSn);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在，请确认" };
            }
            //检查是否有设备查看权限
            if (!IsAdmin)        //非管理员验证权限
            {
                //是否有设备的查看权限
                bool bAuth = await _rps.IsAuth(Roles, data.PathId, 0);
                if (!bAuth)
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                }
            }
            PatrolDataAddDto dto = new PatrolDataAddDto() { DeviceSn = req.DeviceSn, DeviceName = data.DeviceName, Position = req.Position, PositionName = req.PositionName };
            var ret = await _patrolData.AddPatrolDataAsync(Account, Id, dto);
            return ret;
        }
        [HttpPost("Image")]
        [TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<BaseResponse> AddPatrolImageAsync([FromForm] PatrolImageAddRequest req)
        {
            //检测图片类型和图片大小
            foreach (var item in req.file)
            {
                //文件后缀
                var fileExtension = Path.GetExtension(item.FileName);
                //判断后缀是否是图片
                const string fileFilt = ".gif|.jpg|.jpeg|.png";
                if (fileExtension == null)
                {
                    return new BResponse<string> { Success = false, Message = "上传的文件没有后缀", Data = $"{item.FileName}没有后缀名" };
                }
                if (fileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                {
                    return new BResponse<string> { Success = false, Message = "请上传jpg、png、gif格式的图片", Data = $"{item.FileName}格式不正确" };
                }
                //判断文件大小    
                long length = item.Length;
                if (length > 1024 * 1024 * 5) //5M
                {
                    return new BResponse<string> { Success = false, Message = "上传的图片不能大于5M", Data = $"{item.FileName}太大" };
                }
            }
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var data = await _patrolData.IsExist(a => a.Create == Account && a.Id == req.PatrolId);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的巡检单不存在或者用户不是该巡检单的创建者" };
            }
            PatrolImageAddDto dto = new PatrolImageAddDto();
            dto.PatrolId = req.PatrolId;
            //图片保存的相对路径：image+组织编号+ops+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            string userPath = Path.Combine(GroupId, "Ops", "PatrolImage");//保存位置            
            userPath = Path.Combine(_config["StoredImagesPath"], userPath);
            var filePath = Path.Combine(webRootPath, userPath);//物理路径,不包含文件名称
            //如果路径不存在，创建路径
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            List<string> imageUrl = new List<string>();
            try
            {
                foreach (var formFile in req.file)
                {
                    if (formFile.Length > 0)
                    {
                        //var fileExtension = Path.GetExtension(formFile.FileName);
                        string ext = DateTime.Now.ToString("yyyyMMddhhmmss") + formFile.FileName;//图片名称修改为日期加图片名称
                        var imagePath = Path.Combine(filePath, ext);//文件的物理路径
                        imageUrl.Add(Path.Combine(userPath, ext));//图片的相对保存路径
                        using (var stream = System.IO.File.Create(imagePath))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                }
                //文件传入数据库
                //dto.Url = JsonConvert.SerializeObject(imageUrl);
                dto.Url = string.Join(';', imageUrl);
                var ret = await _patrolData.AddPatrolImageAsync(Account, dto);
                if (!ret.Success)//删除已上传的文件
                {
                    //删除已上传的文件
                    foreach (var item in imageUrl)
                    {
                        string url = Path.Combine(filePath, item);
                        if (System.IO.File.Exists(url))
                        {
                            System.IO.File.Delete(url);
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                foreach (var item in imageUrl)
                {
                    string url = Path.Combine(filePath, item);
                    if (System.IO.File.Exists(url))
                    {
                        System.IO.File.Delete(url);
                    }

                }
                _logger.LogError($"{Account}上传巡检图片出错，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传巡检图片出错，请联系管理员" };
            }
        }

        [HttpPost("PatrolItem")]
        [TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<BaseResponse> AddProductDataAsync(PatrolItemAddDto req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var data = await _patrolData.IsExist(a => a.Create == Account && a.Id == req.PatrolId);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的巡检单不存在或者用户不是该巡检单的创建者" };
            }
            var ret = await _patrolData.AddPatrolItemAsync(Account, req);
            return ret;
        }
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<BaseResponse> DeletePatrolDataAsync(string Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var data = await _patrolData.IsExist(a => a.Create == Account && a.Id == Id);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的巡检单不存在或者用户不是该巡检单的创建者" };
            }
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _patrolData.DeletePatrolDataAsync(Account, Id, webRootPath);
            return ret;
        }
        [HttpGet("PatrolItem")]
        [TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<BaseResponse> GetPatrolItemAsync([FromQuery] PatrolItemRequest req)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;

            //检测关联的设备是否存在
            var data = await _ds.CheckDeviceAsync(req.DeviceSn);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在，请确认" };
            }
            var ret = await _patrolData.GetPatrolItemAsync(req, data.TypeId);
            return ret;
        }
        [HttpGet("Page")]
        [TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<BaseResponse> GetPatrolDataPageAsync([FromQuery] PatrolDataRequest req)
        {
            //用户可以看到自己填写的，上级用户可以看到下级用户的填写的
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var data = await _user.GetUserAndChildAsync(Account, IsAdmin);
            if (data.Count <= 0)
            {
                return new BaseResponse { Success = false, Message = "输入的用户不存在" };
            }
            List<string> u = data.Keys.ToList<string>();
            var ret = await _patrolData.GetPatrolDataPageAsync(u, req);
            return ret;
        }
    }
}
