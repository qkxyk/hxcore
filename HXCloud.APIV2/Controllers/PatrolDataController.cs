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
        private readonly IAuthorizationService _authorizationService;//基于资源的授权
        private readonly IModuleOperateService _moduleOperateService;//模块操作
        private readonly IRoleModuleOperateService _roleModuleOperateService;//角色的模块操作
        private readonly IDeviceService _ds;
        private readonly IUserService _user;
        private readonly IPatrolDataService _patrolData;
        private readonly IRoleProjectService _rps;
        private readonly IModuleService _moduleService;
        private readonly IRoleService _role;

        public PatrolDataController(ILogger<PatrolDataController> logger, IConfiguration config, IWebHostEnvironment webHostEnvironment,
             IAuthorizationService authorizationService, IModuleOperateService moduleOperateService, IRoleModuleOperateService roleModuleOperateService,
             IDeviceService ds, IUserService user, IPatrolDataService patrolData, IRoleProjectService rps, IModuleService moduleService, IRoleService role)
        {
            this._logger = logger;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._authorizationService = authorizationService;
            this._moduleOperateService = moduleOperateService;
            this._roleModuleOperateService = roleModuleOperateService;
            this._ds = ds;
            this._user = user;
            this._patrolData = patrolData;
            this._rps = rps;
            this._moduleService = moduleService;
            this._role = role;
        }

        [HttpPost]
        //[TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> AddPatrolDataAsync([FromBody] PatrolDataAddRequest req)
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
            //检查是否有设备查看权限
            if (!IsAdmin)        //非管理员验证权限
            {
                //根据模块code和操作code获取操作标识
                var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "AddPatrol");
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
            PatrolDataAddDto dto = new PatrolDataAddDto() { DeviceSn = req.DeviceSn, DeviceName = data.DeviceName, Position = req.Position, PositionName = req.PositionName };
            dto.ProjectName = data.FullName;
            var op = await _user.GetUserByAccountAsync(Account);
            dto.CreateName = op.UserName;
            var ret = await _patrolData.AddPatrolDataAsync(Account, Id, dto);
            return ret;
        }
        [HttpPost("Image")]
        public async Task<BaseResponse> AddPatrolImageAsync([FromForm] PatrolImageAddRequest req)
        {
            //验证是否是创建巡检单的用户
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var data = await _patrolData.IsExist(a => a.Create == Account && a.Id == req.PatrolId);
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的巡检单不存在或者用户不是该巡检单的创建者" };
            }
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
        //[TypeFilter(typeof(OpsUserFilterAttribute))]
        [Authorize(Policy = "Admin")]//只有管理员能删除
        public async Task<BaseResponse> DeletePatrolDataAsync(string Id)
        {
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            //本人或者上级都可以删除,更改为只能管理员能删除
            var pd = await _patrolData.IsExist(a => a.Id == Id);
            if (pd == null)
            {
                return new BaseResponse { Success = false, Message = "输入的运维单编号不存在" };
            }
            //var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //var users = await _user.GetUserAndChildAsync(Account, IsAdmin);
            //var data = await _patrolData.IsExist(a => users.Keys.Contains(a.Create));
            //if (!data)
            //{
            //    return new BaseResponse { Success = false, Message = "输入的巡检单不存在或者用户不是该巡检单的创建者" };
            //}
            string webRootPath = _webHostEnvironment.WebRootPath;//wwwroot文件夹
            var ret = await _patrolData.DeletePatrolDataAsync(Account, Id, webRootPath);
            return ret;
        }
        [HttpGet("PatrolItem")]
        //[TypeFilter(typeof(OpsUserFilterAttribute))]
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
        /// <summary>
        /// 根据巡检单号获取巡检数据
        /// </summary>
        /// <param name="Id">巡检单号</param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        //[TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetPatrolDataByIdAsync(string Id)
        {
            //查看自己的或者有查看权限
            #region 验证用户权限
            var iss = await _patrolData.IsExistAsync(a => a.Id == Id);
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            if (iss.Create != Account)//自己可以查看自己上报的数据
            {
                var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
                if (!IsAdmin)
                {
                    //验证设备权限
                    var device = await _ds.IsExistAsync(a => a.DeviceSn == iss.DeviceSn);
                    //根据模块code和操作code获取操作标识
                    var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "ViewPatrol");
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
            #endregion
            var ret = await _patrolData.GetPatrolDataByIdAsync(Id);
            return ret;
            #region obsolete
            /*
            //用户可以看到自己填写的，上级用户可以看到下级用户的填写的
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var users = await _user.GetUserAndChildAsync(Account, IsAdmin);
            var data = await _patrolData.IsExist(a => a.Id == Id && users.Keys.Contains(a.Create));
            if (!data)
            {
                return new BaseResponse { Success = false, Message = "输入的巡检单不存在或者用户没有权限查看该巡检单" };
            }
            var ret = await _patrolData.GetPatrolDataByIdAsync(Id);
            return ret;
            */
            #endregion
        }
        [HttpGet("Page")]
        //[TypeFilter(typeof(OpsUserFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetPatrolDataPageAsync([FromQuery] PatrolDataRequest req)
        {
            var ret = new BaseResponse();
            //用户可以看到自己填写的，上级用户可以看到下级用户的填写的
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            #region 验证用户权限
            if (IsAdmin)
            {
                //管理员可以查看所有
                ret = await _patrolData.GetPagePatrolDataAsync(req, true, null, null);
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
                    var operateId = await _moduleOperateService.GetModuleOperateIdByModuleCodeAndOperateCodeAsync("DeviceOps", "ViewPatrol");
                    var RoleOps = await _roleModuleOperateService.GetModuleOperatesAsync(operateId);
                    var ro = RoleOps.FindAll(a => moduleRoles.Contains(a));
                    if (ro == null || ro.Count == 0)
                    {
                        ret = await _patrolData.GetPagePatrolDataAsync(req, false, Account, null);
                    }
                    else
                    {
                        //获取角色分配的项目，以及项目管理的设备
                        var projects = await _rps.GetRoleSitesAsync(ro);
                        //获取设备列表
                        var devices = await _ds.GetDeviceSnBySitesAsync(projects);
                        ret = await _patrolData.GetPagePatrolDataAsync(req, false, null, devices);
                    }
                }
                else
                {
                    return new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                }
            }
            #endregion
            return ret;
            #region obsolete
            /*
            //用户可以看到自己填写的，上级用户可以看到下级用户的填写的
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var data = await _user.GetUserAndChildAsync(Account, IsAdmin);

            //验证输入的用户是否是有权限查看的用户
            if (!string.IsNullOrEmpty(req.UserName))
            {
                var users = await _user.GetUserAndChildNameAsync(Account, IsAdmin);
                if (!users.Contains(req.UserName))
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限查看该用户的巡检数据" };
                }
            }

            if (data.Count <= 0)
            {
                return new BaseResponse { Success = false, Message = "输入的用户不存在" };
            }
            List<string> u = data.Keys.ToList<string>();
            var ret = await _patrolData.GetPatrolDataPageAsync(u, req);
            return ret;
            */
            #endregion
        }
    }
}
