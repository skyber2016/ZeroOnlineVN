using Forum_API.Cores.Exceptions;
using Forum_API.Security;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace Forum_API.Attributes
{
    public class SingleUserAttribute : ActionFilterAttribute
    {
        private IMemoryCache Cache { get; set; }
        private IJwtService JwtService { get; set; }
        public SingleUserAttribute(IMemoryCache cache, IJwtService jwtService)
        {
            this.Cache = cache;
            this.JwtService = jwtService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var attrib = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).Any(x=> x.GetType() == typeof(AllowAnonymousAttribute));
            if (attrib)
            {
                return;
            }
            var header = context.HttpContext.Request.Headers;
            var keyAuth = "Authorization";
            if (header.ContainsKey(keyAuth))
            {
                var jwt = header[keyAuth].ToString();
                if(jwt.StartsWith("Bearer "))
                {
                    jwt = jwt.Remove(0, 7);
                }
                var claim = JwtService.GetTokenClaims(jwt);
                var userId = claim.FirstOrDefault(x => x.Type == "Id").Value;
                var jwtCache = Cache.Get<string>("JWT:" + userId);
                if(jwtCache != jwt) 
                {
                    throw new UnAuthorizeException();
                }
            }
        }
    }
}
