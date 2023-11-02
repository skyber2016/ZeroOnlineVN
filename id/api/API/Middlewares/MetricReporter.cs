using Microsoft.Extensions.Logging;
using Prometheus;
using System;

namespace API.Middlewares
{
    public class MetricReporter
    {
        private readonly Counter _counter;
        private readonly Histogram _histogram;

        public MetricReporter()
        {
            _counter = Metrics.CreateCounter("total_requests", "The total number of requests serviced by this API.");

            _histogram = Metrics.CreateHistogram("request_duration_seconds",
                "The duration in seconds between the response to a request.", new HistogramConfiguration
                {
                    Buckets = Histogram.ExponentialBuckets(0.01, 2, 10),
                    LabelNames = new[] { "path", "method", "status" }
                });
        }

        public void RegisterRequest()
        {
            _counter.Inc();
        }

        public void RegisterResponseTime(string path, string method, int statusCode)
        {
            _histogram.Labels(path, method, statusCode.ToString());
        }

        public void RegisterResponseTime(string path, string method, int statusCode, TimeSpan elapsed)
        {
            _histogram.Labels(path, method, statusCode.ToString()).Observe(elapsed.TotalSeconds);
        }
    }
}
