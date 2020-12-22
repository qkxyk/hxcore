using System;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace HXCloud.APIV2.Filters
{
    //依赖注入方式有三种：
    //1、serviceFilter+startup  在使用是使用[ServiceFilter(typeof(TypeActionFilter))]  在startup中使用services.AddScoped<TypeActionFilter>()
    //2、TypeFilter 在使用的地方使用[TypeFilter(typeof(TypeActionFilter))]
    //3、全局注册
    public class TypeActionFilter : Attribute/*, IActionFilter*/, IAsyncActionFilter
    {
        //public int TypeId { get; set; }
        private readonly IConfiguration _config;
        private readonly ITypeService ts;

        public TypeActionFilter(IConfiguration config, ITypeService ts)
        {
            this._config = config;
            this.ts = ts;
        }
        /*
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var typeParameter = context.ActionArguments.Single(a => a.Key == "typeId");
            int typeId = int.Parse(typeParameter.Value.ToString());
            var IsExist = ts.IsExist(a => a.Id == typeId).Result;
            if (!IsExist)
            {
                context.Result = new NotFoundResult();
            }
            //var method = MethodBase.GetCurrentMethod();
            //var path = context.HttpContext.Request.Path;
            //var type = context.HttpContext;
            //context.Result = new ContentResult { Content = "找不到", StatusCode = 404 };
            //throw new NotImplementedException();
        }
*/
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //同一个组织的合法用户都有权限
            var u = context.HttpContext.User;
            var isAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var GroupId = u.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var code = u.Claims.FirstOrDefault(a => a.Type == "Code").Value;

            var typeParameter = context.ActionArguments.Single(a => a.Key == "typeId");
            int typeId = int.Parse(typeParameter.Value.ToString());
            string GId;
            var IsExist = ts.IsExist(a => a.Id == typeId, out GId);
            if (!IsExist)
            {
                // context.Result = new NotFoundResult();
                context.Result = new NotFoundObjectResult("输入的类型编号不存在");
                return;
            }
            var s = context.HttpContext.Request.Method.ToLower();
            //区分get和其他请求
            switch (s)
            {
                case "get":
                    //用户所在的组和超级管理员可以查看
                    if (GroupId != GId)
                    {
                        if (!(isAdmin && code == _config["Group"]))
                        {
                            context.Result = new UnauthorizedResult();
                            return;
                        }
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
