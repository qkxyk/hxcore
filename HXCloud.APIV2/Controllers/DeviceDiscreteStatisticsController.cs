using HXCloud.Service;
using HXCloud.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Controllers
{
    [Route("api/{GroupId}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceDiscreteStatisticsController : ControllerBase
    {
        private readonly IDeviceDiscreteStatisticsService _dsd;
        private readonly IGroupService _gs;
        private readonly IProjectService _ps;
        private readonly IDeviceService _ds;
        private readonly IConfiguration _config;
        private readonly IRoleProjectService _rps;

        public DeviceDiscreteStatisticsController(IDeviceDiscreteStatisticsService dsd, IGroupService gs, IProjectService ps, IDeviceService ds, IConfiguration config, IRoleProjectService rps)
        {
            this._dsd = dsd;
            this._gs = gs;
            this._ps = ps;
            this._ds = ds;
            this._config = config;
            this._rps = rps;
        }
        /// 获取设备的统计数据
        /// </summary>
        /// <param name="GroupId">组织编号</param>
        /// <param name="req">请求参数</param>
        /// <returns>返回设备的统计数据</returns>
        [HttpGet]
        public async Task<ActionResult<BaseResponse>> GetDeviceStatisticsAsync(string GroupId, [FromQuery] DeviceStatisticsRequestDto req)
        {
            if (req.BeginTime > req.EndTime)
            {
                return new BaseResponse { Success = false, Message = "开始时间不能大于结束时间" };
            }
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").Value.ToString();
            //验证输入的groupid是否存在
            var ex = await _gs.IsExist(a => a.Id == GroupId);
            if (!ex)
            {
                return new BaseResponse { Success = false, Message = "输入的组织编号不存在" };
            }
            if (GId != GroupId)
            {
                if (!(isAdmin && Code == _config["Group"]))
                {
                    return new BaseResponse { Success = false, Message = "用户没有权限" };
                }
            }

            if (req.IsDevice)//设备的权限
            {
                if (req.DeviceSn == null || "" == req.DeviceSn.Trim())
                {
                    return new BaseResponse { Success = false, Message = "设备序列号不能为空" };
                }
                var dev = await _ds.IsExistCheck(a => a.DeviceSn == req.DeviceSn);
                if (!dev.IsExist)
                {
                    return new BaseResponse { Success = false, Message = "输入的设备序列号不存在" };
                }
                //验证权限 
                if (!isAdmin)
                {
                    var auth = await _rps.IsAuth(Roles, dev.PathId, 0);
                    if (!auth)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限查看该设备的统计数据" };
                    }
                }
                //获取设备的统计数据
                return await _dsd.GetDeviceStatisticsAsync(req, null);
            }
            else
            {
                //获取场站列表
                List<int> sites = null;
                if (req.ProjectId == 0)//全部设备
                {
                    sites = await _ps.GetMySitesIdAsync(GroupId, Roles, isAdmin);
                }
                else
                {
                    var pro = await _ps.GetProjectCheckAsync(req.ProjectId);
                    if (!pro.IsExist)
                    {
                        return new BaseResponse { Success = false, Message = "输入的项目或场站编号不存在" };
                    }
                    if (!isAdmin)
                    {
                        pro.PathId = $"{pro.PathId}/{req.ProjectId}";
                        var auth = await _rps.IsAuth(Roles, pro.PathId, 0);
                        if (!auth)
                        {
                            return new BaseResponse { Success = false, Message = "用户没有权限查看该项目下的数据" };
                        }
                    }
                    if (pro.IsSite)
                    {
                        sites = new List<int>() { req.ProjectId };
                    }
                    else
                    {
                        sites = await _ps.GetProjectSitesIdAsync(req.ProjectId);
                    }
                }
                var devices = await _ds.GetDeviceSnBySitesAsync(sites);
                return await _dsd.GetDeviceStatisticsAsync(req, devices);
            }
        }
    }
}
