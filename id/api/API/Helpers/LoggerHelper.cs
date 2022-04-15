using log4net;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace API.Helpers
{
    public interface ILoggerManager
    {
        void Info(string message);
        void Error(string message);
    }

    public class LoggerHelper : ILoggerManager
    {
        private readonly ILog _logFile = LogManager.GetLogger(typeof(LoggerHelper));
        private IHttpContextAccessor Accessor { get; set; }
        private string ThreadId { get; set; }
        private string IPAddress { get; set; }
        public LoggerHelper(IHttpContextAccessor accessor)
        {
            Accessor = accessor;
            this.IPAddress = HttpHelper.GetIP(Accessor);
            this.ThreadId = GenerateThreadID(8);
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
            var mess = $"[{ThreadId}] [{IPAddress}] {message}";
            _logFile.Error(mess);
        }

        public void Info(string message)
        {
            var mess = $"[{ThreadId}] [{IPAddress}] {message}";
            _logFile.Info(mess);
        }
    }


}