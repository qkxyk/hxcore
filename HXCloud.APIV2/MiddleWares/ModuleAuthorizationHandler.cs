using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.MiddleWares
{
    public class ModuleAuthorizationHandler : AuthorizationHandler<ModuleRequirement, ResourceData>
    {
 
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ModuleRequirement requirement, ResourceData resource)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
