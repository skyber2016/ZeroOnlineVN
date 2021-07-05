using CORE_API.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CORE_API.Middlewares
{
    public class PrometheusMiddleware
    {
        private readonly RequestDelegate _request;

        public PrometheusMiddleware(RequestDelegate request)
        {
            _request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public async Task Invoke(HttpContext httpContext, PrometheusReporter reporter)
        {
            var path = httpContext.Request.Path.Value;
            var method = httpContext.Request.Method;
            if (path == "/api/actuator/prometheus")
            {
                await _request.Invoke(httpContext);
                return;
            }
            var sw = Stopwatch.StartNew();

            try
            {
                await _request.Invoke(httpContext);
            }
            finally
            {
                sw.Stop();
                reporter.RegisterRequest();
                reporter.RegisterResponseTime(path, method, httpContext.Response.StatusCode);
            }
        }
    }
}
