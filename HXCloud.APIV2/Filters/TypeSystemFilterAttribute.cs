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
    public class TypeSystemFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly IConfiguration _config;
        private readonly ITypeSystemService _its;

        public TypeSystemFilterAttribute(IConfiguration config, ITypeSystemService its)
        {
            this._config = config;
            this._its = its;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var u = context.HttpContext.User;
            var isAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var GroupId = u.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var code = u.Claims.FirstOrDefault(a => a.Type == "Code").Value;

            var typeParameter = context.ActionArguments.Single(a => a.Key == "systemId");
            int systemId = int.Parse(typeParameter.Value.ToString());
            string GId;
            var IsExist = _its.IsExist(a => a.Id == systemId, out GId);
            if (!IsExist)
            {
                context.Result = new NotFoundResult();
            }
            var s = context.HttpContext.Request.Method.ToLower();
            //区分get和其他请求
            switch (s)
            {
                case "get":
                    //用户所在的组和超级管理员可以查看
                    if (GroupId != GId || (!isAdmin && code != _config["Group"]))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    break;
                default:
                    if (!(isAdmin && (GroupId == GId || code == _config["Group"])))
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                    break;
            }
            await next();
        }
    }
}
