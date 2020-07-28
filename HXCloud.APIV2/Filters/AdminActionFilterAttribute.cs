using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Filters
{
    //同组织管理员或者超级管理员
    public class AdminActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IConfiguration _config;

        public AdminActionFilterAttribute(IConfiguration config)
        {
            this._config = config;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //同一个组织的合法用户都有权限
            var u = context.HttpContext.User;
            var isAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var GroupId = u.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = u.Claims.FirstOrDefault(a => a.Type == "Code").Value;

            var GId = context.ActionArguments.Single(a => a.Key == "GroupId").Value.ToString();
            if (isAdmin && (GId == GroupId || Code == _config["Group"]))
            {
                await next();
            }
            else
            {
                context.Result = new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
            }
        }
    }
}
