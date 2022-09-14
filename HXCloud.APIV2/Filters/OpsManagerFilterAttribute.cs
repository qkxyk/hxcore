﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Filters
{
    /// <summary>
    /// 合法用户
    /// </summary>
    public class OpsManagerFilterAttribute : AuthorizeFilter
    {
        /// <summary>
        /// 验证运维管理人员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await base.OnAuthorizationAsync(context);
            var u = context.HttpContext.User;
            var isAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            //运维人员
            var ops = u.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            //管理员和运维管理人员可以通过
            if (!isAdmin && category < 3)
            {
                context.Result = new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
            }
        }
    }
}
