using log4net;
using System;
using System.Linq;

namespace API.Helpers
{
    public interface ILoggerManager
    {
        void Info(string message);
        void Debug(string message);
        void Error(string message);
        void Data(string message);
        void Queries(string message);
        void Status(string message);
    }

    public class LoggerHelper : ILoggerManager
    {
        private readonly ILog _logFile = LogManager.GetLogger("RollingLogFileAppender");
        private readonly ILog _logQueries = LogManager.GetLogger("Queries");
        private readonly ILog _logStatus = LogManager.GetLogger("Status");
        private readonly ILog _logData = LogManager.GetLogger("Data");
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
            _logFile.Error(message);
        }
        public void Debug(string message)
        {
            _logFile.Debug(message);
        }

        public void Info(string message)
        {
            _logFile.Info(message);
        }

        public void Data(string message)
        {
            _logData.Info(message);
        }

        public void Queries(string message)
        {
            _logQueries.Info(message);
        }

        public void Status(string message)
        {
            _logStatus.Info(message);
        }
    }


}