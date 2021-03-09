using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using API.DTO.Authenticate.Responses;
using API.Services.Interfaces;

namespace API.Configurations
{
    public class AutomationLoginMiddleware
    {
        public static LoginResponse LoginResponse { get; set; }
        private readonly RequestDelegate _next;
        private readonly IAuthService authService;
        public AutomationLoginMiddleware(RequestDelegate next, IAuthService authService)
        {
            this._next = next;
            this.authService = authService;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            if(LoginResponse != null && !httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                httpContext.Request.Headers["Authorization"] = "Bearer " + LoginResponse.Token;
            }
            // Chuyển Middleware tiếp theo trong pipeline
            await _next(httpContext);
        }

    }
}
