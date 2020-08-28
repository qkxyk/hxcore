using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Filters
{
    //超级管理员权限，用于处理报警和数据定义库
    public class SuperAdminFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IConfiguration _config;

        public SuperAdminFilterAttribute(IConfiguration config)
        {
            this._config = config;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //超级管理员有权限
            var u = context.HttpContext.User;
            var isAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var GroupId = u.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = u.Claims.FirstOrDefault(a => a.Type == "Code").Value;

            string Account = u.Claims.FirstOrDefault(a => a.Type == "Account").Value;

            if (!(isAdmin && Code == _config["Group"]))
            {
                context.Result = new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
            }
            else
            {
                await next();
            }
        }
    }
}
