using NEWS_API.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using NEWS_API.Services;
using System.Linq;

namespace NEWS_API.Attributes
{
    public class CachingAttribute : ActionFilterAttribute
    {
        public IWebHostEnvironment Env { get; set; }
        private ActionDescriptor ActionDescriptor { get; set; }
        public CachingAttribute(IWebHostEnvironment env, ActionDescriptor actionDescriptor)
        {           
            this.Env = env;
            this.ActionDescriptor = actionDescriptor;

        }
        /// <summary>
        /// Check Cacheable and Ignore Cache Atribute
        /// </summary>
        /// <param name="context"></param>        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (this.Env.IsDevelopment())
            {
                return;
            }
            var ignoreCache = (context.ActionDescriptor as ControllerActionDescriptor).FilterDescriptors.Any(x => x.Filter.GetType() == typeof(IgnoreCacheAttribute));
            if (ignoreCache)
            {
                this.ActionDescriptor.IsIgnoreCache = ignoreCache;
            }
            ignoreCache = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).Any(x => x.GetType() == typeof(IgnoreCacheAttribute));
            if (ignoreCache)
            {
                this.ActionDescriptor.IsIgnoreCache = ignoreCache;
            }
            if (context.HttpContext.Request.Method.ToUpper() != "GET")
            {
                return;
            }

            var attrib = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).Any(x => x.GetType() == typeof(CacheableAttribute));
            if (attrib)
            {
                var keyCache = context.HttpContext.Request.Path.ToString() + context.HttpContext.Request.QueryString.ToString();
                object response = MemoryCacheHelper.GetValue(keyCache);
                if (response != null)
                {
                    context.Result = new JsonResult(response)
                    {
                        ContentType = "application/json",
                        StatusCode = 200
                    };
                }
                else
                {
                    base.OnActionExecuting(context);
                }
            }
        }
    }
}
