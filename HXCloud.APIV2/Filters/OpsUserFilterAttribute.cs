using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Filters
{
    /// <summary>
    /// 运维用户验证
    /// </summary>
    public class OpsUserFilterAttribute : AuthorizeFilter
    {
        private readonly IConfiguration _config;

        public OpsUserFilterAttribute(IConfiguration config)
        {
            this._config = config;
        }
        /// <summary>
        /// 用户授权验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await base.OnAuthorizationAsync(context);
            var u = context.HttpContext.User;
            var isAdmin = u.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            var GroupId = u.Claims.FirstOrDefault(a => a.Type == "GroupId").Value;
            var Code = u.Claims.FirstOrDefault(a => a.Type == "Code").Value;
            //运维人员
            var ops = u.Claims.FirstOrDefault(a => a.Type == "Category").Value;
            int category = 0;
            int.TryParse(ops, out category);
            //管理员和运维人员可以通过
            if (!isAdmin && category == 0)
            {
                context.Result = new ContentResult { Content = "用户没有权限", ContentType = "text/plain", StatusCode = 401 };
            }
        }
    }
}
