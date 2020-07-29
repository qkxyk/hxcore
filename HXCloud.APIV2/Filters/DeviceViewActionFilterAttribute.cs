using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Filters
{
    public class DeviceViewActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IDeviceService _ds;
        private readonly IRoleProjectService _rps;
        private readonly IConfiguration _config;

        public DeviceViewActionFilterAttribute(IDeviceService ds, IRoleProjectService rps, IConfiguration config)
        {
            this._ds = ds;
            this._rps = rps;
            this._config = config;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //获取用户登录信息
            var u = context.HttpContext.User;
            string name = u.Identity.Name;
            var Roles = u.Claims.FirstOrDefault(a => a.Type == "Role").ToString();
            var Code = u.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            var GroupId = u.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var account = u.Claims.FirstOrDefault(a => a.Type == "Account").Value;
            var IsAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //1、验证是否同组织管理员，2、验证角色权限，3、对比设备上的场站和项目
            //获取设备信息
            //var t = context.ActionArguments.SingleOrDefault(a => a.Key == "devicesn").Value;
            var typeParameter = context.ActionArguments.Single(a => a.Key == "devicesn");
            var device = await _ds.IsExistCheck(a => a.DeviceSn == typeParameter.Value.ToString());
            if (!device.IsExist)
            {
                context.Result = new ContentResult { StatusCode = 404, Content = "输入的设备编号不存在" };
                return;
            }
            //用户所在的组和超级管理员可以查看
            if (GroupId != device.GroupId || (IsAdmin && Code != _config["Group"]))
            {
                context.Result = new ContentResult { StatusCode = 401, Content = "用户没有权限" };
                return;
            }
            if (!IsAdmin)        //非管理员验证权限
            {
                //是否有设备的查看权限
                bool bAuth = await _rps.IsAuth(Roles, device.PathId, 0);
                if (!bAuth)
                {
                    context.Result = new ContentResult { StatusCode = 401, Content = "用户没有权限查看设备的功能" };
                    return;
                }
            }
            if (context.Result == null)
            {
                var resultContext = await next();
            }
        }
    }
}
