using Forum_API.DTO.Authenticate.Responses;
using Forum_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Forum_API.Configurations
{
    public class AutomationLoginMiddleware
    {
        public static LoginResponse LoginResponse { get; set; }
        private readonly RequestDelegate _next;
        public AutomationLoginMiddleware(RequestDelegate next, IAuthService authService)
        {
            this._next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            
            if (LoginResponse != null && !httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                httpContext.Request.Headers["Authorization"] = "Bearer " + LoginResponse.Token;
            }
            // Chuyển Middleware tiếp theo trong pipeline
            await _next(httpContext);
        }

    }
}
