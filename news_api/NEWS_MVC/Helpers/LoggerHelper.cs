using CORE_API.Helpers;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace NEWS_MVC.Helpers
{
    public class LoggerHelper : ILoggerManager
    {
        private readonly ILog _logFile = LogManager.GetLogger(typeof(LoggerHelper));
        private readonly ILogger<LoggerHelper> loggingGrayLog;
        private readonly IConfiguration Configuration;

        private IHttpContextAccessor Accessor { get; set; }
        private string ThreadId { get; set; }
        private string IPAddress
        {
            get
            {
                return HttpHelper.GetIP(Accessor);
            }
        }
        public LoggerHelper(IHttpContextAccessor accessor, ILogger<LoggerHelper> _loggingGrayLog, IConfiguration configuration)
        {
            Accessor = accessor;
            this.loggingGrayLog = _loggingGrayLog;
            this.Configuration = configuration;
            this.ThreadId = GenerateThreadID(8);
            LogicalThreadContext.Properties["requestId"] = $"{ThreadId}";
        }
        private string GenerateThreadID(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        


        public void Error(string message)
        {
            _logFile.Error(message);
        }

        public void Info(string message)
        {
            _logFile.Info(message);
        }

        public string GetLoggerId()
        {
            return this.ThreadId;
        }
    }
}