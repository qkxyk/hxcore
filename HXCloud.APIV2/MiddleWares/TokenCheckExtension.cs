using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.MiddleWares
{
    public static class TokenCheckExtension
    {
        public static IApplicationBuilder UseTokenCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenCheckMiddleware>();
        }
    }
}
