using HXCloud.Service;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.MiddleWares
{
    /// <summary>
    /// 验证用户角色模块的权限
    /// </summary>
    public class ModuleAuthorizationHandler : AuthorizationHandler<ModuleRequirement, ResourceData>
    {
        private readonly IRoleService _roleService;
        private readonly IRoleModuleOperateService _roleModuleOperateService;
        private readonly IRoleProjectService _roleProjectService;

        public ModuleAuthorizationHandler(IRoleService roleService, IRoleModuleOperateService roleModuleOperateService, IRoleProjectService roleProjectService)
        {
            this._roleService = roleService;
            this._roleModuleOperateService = roleModuleOperateService;
            this._roleProjectService = roleProjectService;
        }
        /// <summary>
        /// 验证角色是否有模块的操作权限,可以设置符合条件的用户角色(用户多个角色),并返回有操作权限的角色列表
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModuleRequirement requirement, ResourceData resource)
        {
            var User = context.User;
            var isAdmin = User.Claims.FirstOrDefault(a => a.Type == "IsAdmin").Value.ToLower() == "true" ? true : false;
            if (isAdmin)
            {
                context.Succeed(requirement);//管理员跳过验证
            }
            else
            {
                //非管理员验证用户的角色是否有权限操作，操作权限包含的角色标识根据上下文得出
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
                if (requirement.ModuleRoles.Count>0)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
