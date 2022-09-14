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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IIssueService _issue;
        private readonly ILogger<IssueController> _log;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDeviceService _ds;
        private readonly IUserService _user;
        private readonly IRoleProjectService _rps;

        /// <summary>
        /// 问题单
        /// </summary>
        /// <param name="issue"></param>
        /// <param name="log"></param>
        /// <param name="config"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="ds"></param>
        /// <param name="user"></param>
        public IssueController(IIssueService issue, ILogger<IssueController> log, IConfiguration config, IWebHostEnvironment webHostEnvironment, IDeviceService ds, IUserService user, IRoleProjectService rps)
        {
            this._issue = issue;
            this._log = log;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._ds = ds;
            this._user = user;
            this._rps = rps;
        }
        /// <summary>
        /// 录入问题单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddIssueAsync([FromForm] IssueAddRequest req)
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
                    return new BResponse<string> { Success = false, Message = "上传的文件没有后缀", Data=$"{item.FileName}没有后缀名" };
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
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            IssueAddDto dto = new IssueAddDto();

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
            dto.DeviceName = data.DeviceName;
            dto.DeviceSn = req.DeviceSn;
            dto.Description = req.Description;
            //图片保存的相对路径：image+组织编号+ops+图片名称
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            string userPath = Path.Combine(GroupId, "Ops", "IssueImage");//保存位置    
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
                dto.Url = JsonConvert.SerializeObject(imageUrl);
                var ret = await _issue.AddIssueAsync(Account, dto);
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
                _log.LogError($"{Account}上传问题文件出错，失败原因{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
                return new BaseResponse { Success = false, Message = "上传数据出错，请联系管理员" };
            }
        }

        /// <summary>
        /// 获取用户的问题单，分页数据（可以获取到包含用户下级的问题单）
        /// </summary>
        /// <param name="req">可根据设备名称模糊搜索</param>
        /// <returns>返回用户问题单列表</returns>
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetIssueAsync([FromQuery] IssuePageRequest req)
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
            var ret = await _issue.GetIssuePageRequestAsync(u, req);
            return ret;
        }

        [HttpPut]
        [TypeFilter(typeof(OpsManagerFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateIssueAsync([FromBody] IssueUpdateDto req)
        {
            //更改问题单状态
            //获取用户登录信息,运维管理员以上可以处理问题单
            //var u = context.HttpContext.User;
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            //int category = 0;
            //int.TryParse(ops, out category);
            //if (IsAdmin | category == 4)
            //{
            var ret = await _issue.UpdateIssueAsync(account, req);
            return ret;
            //}
            //else
            //{
            //    return new BaseResponse { Success = false, Message = "用户没有权限处理问题单" };
            //}
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteIssueAsync(int Id)
        {
            //用户只能删除没有处理过并且只能是自己提交的问题单，只有管理员才能删除问题单
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //int category = 0;
            //int.TryParse(User.Claims.FirstOrDefault(a => a.Type == "Category").Value, out category);
            var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            //删除问题单
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _issue.DeleteIssueAsync(account, Id, IsAdmin, category, webRootPath);
            return ret;
        }
    }
}
