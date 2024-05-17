using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.MiddleWares
{
    public class CheckModuleRequirement : IAuthorizationRequirement
    {
        public CheckModuleRequirement(List<int> roles)
        {
            Roles = roles;
        }
        public List<int> Roles { get; set; }//模块权限的角色(验证权限时传入的角色列表)
        public List<int> ModuleRoles { get; set; } = new List<int>();//用户有模块权限的角色(返回，用于根据角色获取用户的项目)
    }

    /// <summary>
    /// 分拣出用户属于某一模块的角色列表
    /// </summary>
    public class CheckModuleAuthorizationHandler : AuthorizationHandler<CheckModuleRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CheckModuleRequirement requirement)
        {
            var User = context.User;
            var roles = User.Claims.Where(a => a.Type == "Role");
            //比对用户的角色
            foreach (var item in roles)
            {
                int role = int.Parse(item.Value);
                //检测角色是否属于该模块
                var ex = requirement.Roles.Contains(role);
                if (ex)
                {
                    //有操作权限的角色分拣出来，返回处理
                    requirement.ModuleRoles.Add(role);
                }
            }
            if (requirement.ModuleRoles.Count > 0)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
