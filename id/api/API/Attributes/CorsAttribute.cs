using API.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Attributes
{
    public class CorsAttribute : ActionFilterAttribute
    {
        private readonly ILoggerManager Logger;
        public CorsAttribute(ILoggerManager logger)
        {
            this.Logger = logger;
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            this.Logger.Info("Set header cors");
            context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            if (context.HttpContext.Request.Method == "OPTIONS")
            {
                //These headers are handling the "pre-flight" OPTIONS call sent by the browser
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
                context.HttpContext.Response.Headers.Add("Access-Control-Max-Age", "1728000");
            }
            base.OnResultExecuted(context);
        }
    }
}
