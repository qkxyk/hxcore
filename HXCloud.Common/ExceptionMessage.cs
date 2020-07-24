using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HXCloud.Common
{
    public class ExceptionMessage
    {
        //private object errorMessage;
        public string Message { get; private set; }
        public string Description { get; private set; }
        public IDictionary<string, string> ValidationErrors { get; private set; }
        public ExceptionMessage(ExceptionContext context)
        {
            if (context.ModelState != null && context.ModelState.Any(m => m.Value.Errors.Any()))
            {
                this.Message = "Model validation failed.";
                this.ValidationErrors = context.ModelState.Keys
                    .SelectMany(key => context.ModelState[key].Errors.ToDictionary(k => key, v => v.ErrorMessage))
                    .ToDictionary(k => k.Key, v => v.Value);
            }
            else
            {
                this.Message = context.Exception.Message;
                this.Description = context.Exception.StackTrace;
            }
        }
    }
}
