using log4net;
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
        private string ThreadId { get; set; }
        public LoggerHelper()
        {
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
            var mess = $"[{ThreadId}] {message}";
            _logFile.Error(mess);
        }

        public void Info(string message)
        {
            var mess = $"[{ThreadId}] {message}";
            _logFile.Info(mess);
        }
    }


}