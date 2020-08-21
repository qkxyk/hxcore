using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HXCloud.APIV2.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
/* 处理服务异常
 * Author:xzc 20200813
 * 
 */
namespace HXCloud.APIV2.Filters
{
    public class JsonExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<Program> _log;

        public JsonExceptionFilter(IWebHostEnvironment env, ILogger<Program> log)
        {
            this._env = env;
            this._log = log;
        }
        public void OnException(ExceptionContext context)
        {
            ApiError error = new ApiError();
            var ex = context.Exception;
            if (_env.IsDevelopment())
            {
                error.Message = ex.Message;
                error.Detail = ex.ToString();
            }
            else
            {
                error.Message = "服务器故障";
                error.Detail = ex.Message;
            }
            context.Result = new ObjectResult(error)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            _log.LogError($"服务发生异常：{ex.Message}->{ex.StackTrace}->{ex.InnerException}");
        }
    }
}
