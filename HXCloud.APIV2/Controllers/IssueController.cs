using HXCloud.APIV2.Filters;
using HXCloud.APIV2.MiddleWares;
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
        private readonly IAuthorizationService _authorizationService;
        private readonly IModuleOperateService _moduleOperateService;
        private readonly IRoleModuleOperateService _roleModuleOperateService;
        private readonly IDeviceService _ds;
        private readonly IUserService _user;
        private readonly IRoleProjectService _rps;
        private readonly IModuleService _moduleService;
        private readonly IRoleService _role;

        /// <summary>
        /// 问题单
        /// </summary>
        /// <param name="issue"></param>
        /// <param name="log"></param>
        /// <param name="config"></param>
        /// <param name="webHostEnvironment"></param>
        /// <param name="authorizationService"></param>
        /// <param name="moduleOperateService"></param>
        /// <param name="roleModuleOperateService"></param>
        /// <param name="ds"></param>
        /// <param name="user"></param>
        /// <param name="rps"></param>
        /// <param name="moduleService"></param>
        /// <param name="role"></param>
        public IssueController(IIssueService issue, ILogger<IssueController> log, IConfiguration config, IWebHostEnvironment webHostEnvironment,
               IAuthorizationService authorizationService, IModuleOperateService moduleOperateService, IRoleModuleOperateService roleModuleOperateService,
               IDeviceService ds, IUserService user, IRoleProjectService rps, IModuleService moduleService, IRoleService role)
        {
            this._issue = issue;
            this._log = log;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._authorizationService = authorizationService;
            this._moduleOperateService = moduleOperateService;
            this._roleModuleOperateService = roleModuleOperateService;
            this._ds = ds;
            this._user = user;
            this._rps = rps;
            this._moduleService = moduleService;
            this._role = role;
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
            //检测关联的设备是否存在
            var data = await _ds.CheckDeviceAsync(req.DeviceSn);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备不存在，请确认" };
            }

            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            int Id = 0;
            int.TryParse(User.Claims.FirstOrDefault(a => a.Type == "Id").Value, out Id);
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();

            //检查是否有设备查看权限
            if (!IsAdmin)        //非管理员验证权限
            {
                //根据模块code和操作code获取操作标识
                var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "AddIssue");
                if (operateId == 0)
                {
                    return new BaseResponse { Success = false, Message = "出现错误，请联系管理员" };
                }
                //获取模块分配的角色操作
                var rp = await _roleModuleOperateService.GetModuleOperatesAsync(operateId);
                var mr = new ModuleRequirement(2, "export", rp);//目前前两个参数无意义
                ResourceData resource = new ResourceData { Compare = CompareData.Equal, Operate = 1, ProjectId = 1024 };
                var t = await _authorizationService.AuthorizeAsync(User, resource, mr);
                if (t.Succeeded)
                {
                    //验证用户在该模块中的角色是否对该设备有查看权限
                    var role = string.Join(',', mr.ModuleRoles);
                    bool bAuth = await _rps.IsAuth(role, data.PathId, 0);
                    if (!bAuth)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                    }
                }
                else
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                }
            }
            IssueAddDto dto = new IssueAddDto();
            var op = await _user.GetUserByAccountAsync(Account);
            dto.CreateName = op.UserName;
            dto.ProjectName = data.FullName;
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

        [HttpGet("{Id}")]
        public async Task<ActionResult<BaseResponse>> GetIssueByIdAsync(int Id)
        {
            //查看自己的或者有查看权限
            var iss = await _issue.IsExistAsync(a => a.Id == Id);
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (iss.Create != Account)
            {
                var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
                if (!IsAdmin)
                {
                    //验证设备权限
                    var device = await _ds.IsExistAsync(a => a.DeviceSn == iss.DeviceSn);
                    //根据模块code和操作code获取操作标识
                    var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "ViewIssue");
                    if (operateId == 0)
                    {
                        return new BaseResponse { Success = false, Message = "出现错误，请联系管理员" };
                    }
                    //获取模块分配的角色操作
                    var rp = await _roleModuleOperateService.GetModuleOperatesAsync(operateId);
                    var mr = new ModuleRequirement(2, "export", rp);//目前前两个参数无意义
                    ResourceData resource = new ResourceData { Compare = CompareData.Equal, Operate = 1, ProjectId = 1024 };
                    var t = await _authorizationService.AuthorizeAsync(User, resource, mr);
                    if (t.Succeeded)
                    {
                        //验证用户在该模块中的角色是否对该设备有查看权限
                        var role = string.Join(',', mr.ModuleRoles);
                        bool bAuth = await _rps.IsAuth(role, device.FullId, 0);
                        if (!bAuth)
                        {
                            return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                        }
                    }
                    else
                    {
                        return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                    }
                }
            }
            var ret = await _issue.GetIssueByIdAsync(Id);
            return ret;
            #region obsolete
            /*
            //用户可以看到自己填写的，上级用户可以看到下级用户的填写的
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var data = await _user.GetUserAndChildAsync(Account, IsAdmin);
            List<string> users = data.Keys.ToList();
            //bool exist = await _issue.IsExist(a => a.Id == Id && users.Contains(a.Create));
            //if (!exist)
            //{
            //    return new BaseResponse { Success = false, Message = "问题单不存在或者用户没有权限查看该问题单" };
            //}
            var ret = await _issue.GetIssueByIdAsync(Id, users);
            return ret;
            */
            #endregion
        }

        /// <summary>
        /// 获取用户的问题单，分页数据（可以获取到包含用户下级的问题单）
        /// </summary>
        /// <param name="req">可根据设备名称模糊搜索</param>
        /// <returns>返回用户问题单列表</returns>
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetIssueAsync([FromQuery] IssuePageRequest req)
        {
            var ret = new BaseResponse();
            //用户可以看到自己填写的，上级用户可以看到下级用户的填写的
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            #region 验证用户权限
            if (IsAdmin)
            {
                //管理员可以查看所有
                ret = await _issue.GetPageIssueAsync(req, true, null, null);
            }
            else
            {
                //验证是否有查看权限
                //1、查看用户在运维模块中的角色，没有分配查看的用户则获取自己接单的数据
                var moduleId = await _moduleService.GetModuleIdByCodeAsync("DeviceOps");//获取模块标识
                var role = await _role.GetRoles(a => a.ModuleId == moduleId);//获取模块角色
                CheckModuleRequirement mr = new CheckModuleRequirement(role);
                var t = await _authorizationService.AuthorizeAsync(User, null, mr);//返回用户在该模块的角色
                if (t.Succeeded)
                {
                    var moduleRoles = mr.ModuleRoles;
                    //检测用户角色时候有该模块的查看权限
                    var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "ViewIssue");
                    var RoleOps = await _roleModuleOperateService.GetModuleOperatesAsync(operateId);
                    var ro = RoleOps.FindAll(a => moduleRoles.Contains(a));
                    if (ro == null || ro.Count == 0)
                    {
                        ret = await _issue.GetPageIssueAsync(req, false, Account, null);
                    }
                    else
                    {
                        //获取角色分配的项目，以及项目管理的设备
                        var projects = await _rps.GetRoleSitesAsync(ro);
                        //获取设备列表
                        var devices = await _ds.GetDeviceSnBySitesAsync(projects);
                        ret = await _issue.GetPageIssueAsync(req, false, null, devices);
                    }
                }
                else
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                }
            }
            return ret;
            #endregion
            /*
            var data = await _user.GetUserAndChildAsync(Account, IsAdmin);
            if (data.Count <= 0)
            {
                return new BaseResponse { Success = false, Message = "输入的用户不存在" };
            }
            List<string> u = data.Keys.ToList<string>();
            var ret = await _issue.GetIssuePageRequestAsync(u, req);
            
            return ret;
            */
        }

        /// <summary>
        /// 处理问题单
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [TypeFilter(typeof(OpsManagerFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateIssueAsync([FromBody] IssueUpdateRequest req)
        {
            var iss = await _issue.IsExistAsync(a => a.Id == req.Id);
            if (iss == null)
            {
                return new BaseResponse { Success = false, Message = "输入的问题单不存在" };
            }
            var data = await _ds.CheckDeviceAsync(iss.DeviceSn);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的问题单关联的设备不存在，请确认问题单是否合理" };
            }
            //验证用户是否有权限
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            if (!IsAdmin)        //非管理员验证权限
            {
                //根据模块code和操作code获取操作标识
                var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "HandleIssue");
                if (operateId == 0)
                {
                    return new BaseResponse { Success = false, Message = "出现错误，请联系管理员" };
                }
                //获取模块分配的角色操作
                var rp = await _roleModuleOperateService.GetModuleOperatesAsync(operateId);
                var mr = new ModuleRequirement(2, "export", rp);//目前前两个参数无意义
                ResourceData resource = new ResourceData { Compare = CompareData.Equal, Operate = 1, ProjectId = 1024 };
                var t = await _authorizationService.AuthorizeAsync(User, resource, mr);
                if (t.Succeeded)
                {
                    //验证用户在该模块中的角色是否对该设备有查看权限
                    var role = string.Join(',', mr.ModuleRoles);
                    bool bAuth = await _rps.IsAuth(role, data.PathId, 0);
                    if (!bAuth)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                    }
                }
                else
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                }
            }
            IssueUpdateDto dto = new IssueUpdateDto();
            dto.Id = req.Id;
            dto.Opinion = req.Opinion;
            dto.Status = req.Status;
            var op = await _user.GetUserByAccountAsync(Account);
            dto.HandleName = op.UserName;
            var ret = await _issue.UpdateIssueAsync(Account, dto);
            return ret;
        }
        [HttpDelete("{Id}")]
        public async Task<ActionResult<BaseResponse>> DeleteIssueAsync(int Id)
        {
            var iss = await _issue.IsExistAsync(a => a.Id == Id);
            if (iss == null)
            {
                return new BaseResponse { Success = false, Message = "输入的问题单不存在" };
            }
            if (iss.Status)
            {
                return new BaseResponse { Success = false, Message = "输入的问题单已处理，不能删除" };
            }
            var data = await _ds.CheckDeviceAsync(iss.DeviceSn);
            if (!data.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的问题单关联的设备不存在，请确认问题单是否合理" };
            }
            //验证用户是否有权限
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            if (!IsAdmin)        //非管理员验证权限
            {
                //根据模块code和操作code获取操作标识
                var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "DeleteIssue");
                if (operateId == 0)
                {
                    return new BaseResponse { Success = false, Message = "出现错误，请联系管理员" };
                }
                //获取模块分配的角色操作
                var rp = await _roleModuleOperateService.GetModuleOperatesAsync(operateId);
                var mr = new ModuleRequirement(2, "export", rp);//目前前两个参数无意义
                ResourceData resource = new ResourceData { Compare = CompareData.Equal, Operate = 1, ProjectId = 1024 };
                var t = await _authorizationService.AuthorizeAsync(User, resource, mr);
                if (t.Succeeded)
                {
                    //验证用户在该模块中的角色是否对该设备有查看权限
                    var role = string.Join(',', mr.ModuleRoles);
                    bool bAuth = await _rps.IsAuth(role, data.PathId, 0);
                    if (!bAuth)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限查看设备的功能" };
                    }
                }
                else
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                }
            }

            //用户只能删除没有处理过并且只能是自己提交的问题单，只有管理员才能删除问题单
            //var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //int category = 0;
            //int.TryParse(User.Claims.FirstOrDefault(a => a.Type == "Category").Value, out category);
            var ops = User.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            //删除问题单
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _issue.DeleteIssueAsync(Account, Id, webRootPath);
            return ret;
        }
    }
}
