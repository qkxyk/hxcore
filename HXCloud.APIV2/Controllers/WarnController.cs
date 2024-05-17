using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Filters;
using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{DeviceSn}/[controller]")]
    [ApiController]
    [Authorize]
    public class WarnController : ControllerBase
    {
        private readonly IWarnService _ws;
        private readonly IProjectService _ps;
        private readonly IConfiguration _config;
        private readonly IRoleProjectService _rp;
        private readonly IDeviceService _ds;
        private readonly IUserRoleService _userRole;

        public WarnController(IWarnService ws, IProjectService ps, IConfiguration config, IRoleProjectService rp, IDeviceService ds, IUserRoleService userRole)
        {
            this._ws = ws;
            this._ps = ps;
            this._config = config;
            this._rp = rp;
            this._ds = ds;
            this._userRole = userRole;
        }

        //获取单个报警信息
        [HttpGet("{Id}")]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetWarnById(string DeviceSn, int Id)
        {
            var data = await _ws.GetWarnById(Id);
            return data;
        }
        //获取设备的报警信息(设备报警)
        [HttpGet]
        [TypeFilter(typeof(DeviceViewActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceWarn(string DeviceSn, [FromQuery] DeviceWarnPageRequest req)
        {
            var data = await _ws.GetWarnByDeviceSnAsync(DeviceSn, req);
            return data;
        }
        [HttpPut]
        [TypeFilter(typeof(DeviceActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> UpdateWarn(string DeviceSn, WarnUpdateDto req)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var data = await _ws.UpdateWarnInfo(account, req);
            return data;
        }
        //获取项目或者场站的报警信息,如果项目编号为空，则为登录账号所有的报警信息
        [HttpGet("/api/[controller]")]
        public async Task<ActionResult<BaseResponse>> GetProjectWarn([FromQuery] ProjectWarnPageRequest req)
        {
            DateTime begin, end;
            bool state = false;
            if (req.BeginTime == null)
            {
                begin = DateTime.Now.AddDays(-1);
            }
            else
            {
                begin = req.BeginTime;
            }
            if (req.EndTime == null)
            {
                end = DateTime.Now;
            }
            else
            {
                end = req.EndTime;
            }
            if (req.BeginTime >= req.EndTime)
            {
                return new BaseResponse { Success = false, Message = "开始时间不能大于或者等于结束时间" };
            }
            if (req.State == 0)
            {
                state = false;
            }
            else
            {
                state = true;
            }
            //var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            //获取用户的角色
            var UserId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var Roles = await _userRole.GetUserRolesAsync(UserId);
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var au = User.Identity.IsAuthenticated;
            List<string> Devices;
            if (req.ProjectId.HasValue && req.ProjectId.Value != 0)
            {
                //验证用户权限
                var pro = await _ps.GetProjectCheckAsync(req.ProjectId.Value);
                if (pro.IsExist == false)
                {
                    return new BaseResponse { Success = false, Message = "输入的项目或者场站编号不存在" };
                }
                if (GroupId != pro.GroupId)//不是同一个组织
                {
                    if (!(IsAdmin && Code == _config["Group"]))
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限查看" };
                    }
                }
                else//同一个组织
                {
                    if (!IsAdmin)
                    {
                        pro.PathId = $"{pro.PathId}/{req.ProjectId.Value}";
                        bool isAuth = await _rp.IsAuth(Roles, pro.PathId, 0);
                        if (!isAuth)
                        {
                            return new BaseResponse { Success = false, Message = "用户没有权限查看" };
                        }
                    }
                }
                //获取项目或者场站下的所有的设备编号
                Devices = await _ds.GetProjectDeviceSnAsync(req.ProjectId.Value, pro.IsSite);
            }
            else
            {
                //获取我的设备编号
                Devices = await _ds.GetMyDeviceSnAsync(GroupId, Roles, IsAdmin);
            }
            var data = await _ws.GetProjectWarnAsync(Devices, begin, end, state, new BasePageRequest
            {
                PageNo = req.PageNo,
                PageSize = req.PageSize,
                Search = req.Search,
                OrderBy = req.OrderBy,
                OrderType = req.OrderType
            });
            return data;
        }
        //获取报警统计信息（项目、场站或者全部),按类型统计个数
        [HttpGet("/api/[controller]/statistics")]
        public async Task<ActionResult<BaseResponse>> GetMyWarnStatistics([FromQuery] WarnStatisticsDto req)
        {
            //var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var GroupId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //获取用户的角色
            var UserId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var Roles = await _userRole.GetUserRolesAsync(UserId);
            List<string> Devices = new List<string>();
            if (req.IsDevice)   //统计设备
            {
                var device = await _ds.IsExistCheck(a => a.DeviceSn == req.DeviceSn);
                if (!device.IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的设备序列号不存在" };
                }
                #region //验证对设备的权限
                if (GroupId != device.GroupId)
                {
                    if (!(IsAdmin && Code == _config["Group"]))
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限查看改设备信息" };
                    }
                }
                else
                {
                    if (!IsAdmin)
                    {
                        bool isAuth = await _rp.IsAuth(Roles, device.PathId, 0);
                        if (!isAuth)
                        {
                            return new BaseResponse { Success = false, Message = "用户没有权限查看改设备信息" };
                        }
                    }
                }
                #endregion
                Devices.Add(req.DeviceSn);
            }
            else//统计项目、场站或者全部
            {
                if (req.ProjectId.HasValue && req.ProjectId != 0)//项目或者场站的统计
                {
                    var project = await _ps.GetProjectCheckAsync(req.ProjectId.Value);
                    if (!project.IsExist)
                    {
                        return new BaseResponse { Success = false, Message = "输入的项目或者场站不存在" };
                    }
                    #region 验证权限
                    if (GroupId != project.GroupId)//不是同一个组织
                    {
                        if (!(IsAdmin && Code == _config["Group"]))
                        {
                            return new BaseResponse { Success = false, Message = "用户没有权限查看" };
                        }
                    }
                    else//同一个组织
                    {
                        if (!IsAdmin)
                        {
                            project.PathId = $"{project.PathId}/{req.ProjectId.Value}";
                            bool isAuth = await _rp.IsAuth(Roles, project.PathId, 0);
                            if (!isAuth)
                            {
                                return new BaseResponse { Success = false, Message = "用户没有权限查看" };
                            }
                        }
                    }
                    #endregion
                    //获取项目或者场站下的所有的设备编号
                    Devices = await _ds.GetProjectDeviceSnAsync(req.ProjectId.Value, project.IsSite);
                }
                else//全部设备的统计
                {
                    //获取我的设备编号
                    Devices = await _ds.GetMyDeviceSnAsync(GroupId, Roles, IsAdmin);
                }
            }
            var data = await _ws.GetWarnStatisticsAsync(Devices);
            return data;
        }

        /// <summary>
        /// 获取我的未处理报警数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/MyWarnCount/[controller]")]
        public async Task<ActionResult<BaseResponse>> GetMyWarnCount()
        {
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            string Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            string Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            //string Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value;
            //获取用户的角色
            var UserId = Convert.ToInt32(User.Claims.FirstOrDefault(a => a.Type == "Id").Value);
            var Roles = await _userRole.GetUserRolesAsync(UserId);
            List<string> Devices = new List<string>();
            //获取我的设备编号
            Devices = await _ds.GetMyDeviceSnAsync(GId, Roles, IsAdmin);
            var ret = await _ws.GetWarnCount(Devices);
            return ret;
        }
    }
}