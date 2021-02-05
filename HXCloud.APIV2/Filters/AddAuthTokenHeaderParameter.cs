using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HXCloud.APIV2.Filters
{
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();
            var attrs = context.ApiDescription.CustomAttributes();//.GetActionAttributes();
            foreach (var attr in attrs)
            {
                // 如果 Attribute 是我们自定义的验证过滤器
                //if (attr.GetType() == typeof(AuthorizeAttribute))
                //{
                //    operation.Parameters.Add(new NonBodyParameter()
                //    {
                //        Name = "AuthToken",
                //        In = "header",
                //        Type = "string",
                //        Required = false
                //    });
                //}
            }
        }
    }
}
