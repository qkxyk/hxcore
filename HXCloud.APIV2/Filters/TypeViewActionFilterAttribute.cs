using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Filters
{
    public class TypeViewActionFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly ITypeService _ts;
        private readonly IConfiguration _config;

        public TypeViewActionFilterAttribute(ITypeService ts, IConfiguration config)
        {
            this._ts = ts;
            this._config = config;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //获取传递的参数
            var key = context.HttpContext.Request.Query.Keys;
            //验证是否已经登录
            var u = context.HttpContext.User;
            if (u.Identity.IsAuthenticated)
            {
                //同一个组织的合法用户都有权限
                var isAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
                var GroupId = u.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
                var code = u.Claims.FirstOrDefault(a => a.Type == "Code").Value;
                var s = context.HttpContext.Request.Method;
                if (s.ToLower() == "get")       //合法用户都可以访问
                {

                }
                else if (s.ToLower() == "delete")
                {
                    var Id = Convert.ToInt32(context.ActionArguments.Single(a => a.Key == "Id").Value);
                    string gId;
                    var b = _ts.IsExist(a => a.Id == Id, out gId);
                    if (!b)
                    {
                        context.Result = new ContentResult { Content = "输入的类型编号不存在", ContentType = "text/plain", StatusCode = 400 };
                        return;
                    }
                    if (!(isAdmin && (gId == GroupId || code == _config["Code"])))
                    {
                        context.Result = new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                        return;
                    }
                }
                else//修改
                {
                    var Id = Convert.ToInt32(context.ActionArguments.Single(a => a.Key == "Id").Value);
                    string gId;
                    var b = _ts.IsExist(a => a.Id == Id, out gId);
                    if (!b)
                    {
                        context.Result = new ContentResult { Content = "输入的类型编号不存在", ContentType = "text/plain", StatusCode = 400 };
                        return;
                    }
                    if (!(isAdmin && (gId == GroupId || code == _config["Code"])))
                    {
                        context.Result = new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
                        return;
                    }
                }
                await next();
            }
            else
            {
                context.Result = new ContentResult { Content = "用户认证失败", ContentType = "text/plain", StatusCode = 401 };
            }
        }
    }
}
