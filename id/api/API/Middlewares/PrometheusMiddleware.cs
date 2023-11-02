using Microsoft.AspNetCore.Http;
using Prometheus;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public class PrometheusMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Histogram _requestDurationHistogram;
        public PrometheusMiddleware(RequestDelegate next)
        {
            _next = next;
            _requestDurationHistogram = Metrics.CreateHistogram(
            "api_request_duration_seconds",
            "API Request Duration in Seconds",
            new HistogramConfiguration
            {
                LabelNames = new[] { "path", "method", "status_code" }
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;

            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            // Ghi thời gian thực hiện vào histogram
            _requestDurationHistogram
                .WithLabels(path, context.Request.Method, context.Response.StatusCode.ToString())
                .Observe(stopwatch.Elapsed.TotalSeconds);
        }

    }
}
