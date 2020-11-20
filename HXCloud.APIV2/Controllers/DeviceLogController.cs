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
    /// <summary>
    /// 设备操作日志，有设备控制权限的用户可以添加，合法用户可以获取，管理员可以删除
    /// </summary>
    [Route("api/{GroupId}/{DeviceSn}/[controller]")]
    [ApiController]
    [Authorize]
    public class DeviceLogController : ControllerBase
    {
        private readonly IDeviceLogService _dls;
        private readonly IDeviceService _ds;
        private readonly IRoleProjectService _rs;
        private readonly IConfiguration _config;

        public DeviceLogController(IDeviceLogService dls, IDeviceService ds, IRoleProjectService rs, IConfiguration config)
        {
            this._dls = dls;
            this._ds = ds;
            this._rs = rs;
            this._config = config;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddDeviceLogAsync(string GroupId, string DeviceSn, [FromBody] DeviceLogAddDto req)
        {
            //有用户控制权限的可以操作
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var Roles = User.Claims.FirstOrDefault(a => a.Type == "Role").ToString();
            var Code = User.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var GId = User.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var IsAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var device = await _ds.IsExistCheck(a => a.DeviceSn == DeviceSn);
            if (!device.IsExist)
            {
                return new BaseResponse { Success = false, Message = "输入的设备编号不存在" };
            }
            if (GId != GroupId)
            {
                if (!(IsAdmin && Code != _config["Group"]))
                {
                    return Unauthorized("用户没有权限");
                }
            }
            else
            {
                if (!IsAdmin)
                {
                    bool bAuth = await _rs.IsAuth(Roles, device.PathId, 1);
                    if (!bAuth)
                    {
                        return new BaseResponse { Success = false, Message = "用户没有权限操作设备" };
                    }
                }
            }
            var rm = await _dls.AddDeviceLogAsync(account, DeviceSn, req);
            return rm;
        }
        /// <summary>
        /// 删除设备操作日志，只有管理员有权限
        /// </summary>
        /// <param name="GroupId">组织编号</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="Id">操作日志编号</param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> DeleteDeviceLogAsync(string GroupId, string DeviceSn, int Id)
        {
            string account = User.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var rm = await _dls.RemoveDeviceLogAsync(account, Id);
            return rm;
        }

        /// <summary>
        /// 获取设备的操作日志,只有管理员能查看设备的操作日志
        /// </summary>
        /// <param name="GroupId">组织标示</param>
        /// <param name="DeviceSn">设备序列号</param>
        /// <param name="req">分页请求，默认为前一个月到当前时间的数据</param>
        /// <returns>获取设备的操作日志</returns>
        [HttpGet]
        [TypeFilter(typeof(AdminActionFilterAttribute))]
        public async Task<ActionResult<BaseResponse>> GetDeviceLogAsync(string GroupId, string DeviceSn, [FromQuery] DeviceLogPageRequest req)
        {
            var rm = await _dls.GetDeviceLogsAsync(DeviceSn, req);
            return rm;
        }
    }
}