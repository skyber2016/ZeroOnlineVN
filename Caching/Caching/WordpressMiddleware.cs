using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Caching
{
    public class WordpressMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly AppSettings _appSettings;
        public WordpressMiddleware(RequestDelegate next, IMemoryCache cache, IOptions<AppSettings> options) 
        {
            this._next = next;
            this._cache = cache;
            this._appSettings = options.Value;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            var url = httpContext.Request.Path.Value + httpContext.Request.QueryString.Value;
            var response = this._cache.Get<string>(url);
            if(response == null)
            {
                using(var http = new HttpClient())
                {
                    http.BaseAddress = new Uri(this._appSettings.Wordpress);
                    var httpResponse = await http.GetAsync(url);
                    response = await httpResponse.Content.ReadAsStringAsync();
                    if (httpResponse.IsSuccessStatusCode)
                    {
                        this._cache.Set(url, response, TimeSpan.FromMinutes(30));
                    }
                    else
                    {
                        httpContext.Response.StatusCode = (int)httpResponse.StatusCode;
                        await httpContext.Response.WriteAsync(response);
                        return;
                    }
                }
            }
            httpContext.Response.StatusCode = StatusCodes.Status200OK;
            await httpContext.Response.WriteAsync(response.ToString());
        }
    }
}
