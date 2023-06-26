using log4net;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace API.Helpers
{
    public interface ILoggerManager
    {
        void Info(string message, [CallerMemberName] string memberName = "");
        void Debug(string message, [CallerMemberName] string memberName = "");
        void Error(string message, [CallerMemberName] string memberName = "");
        void Data(string message, [CallerMemberName] string memberName = "");
        void Queries(string message, [CallerMemberName] string memberName = "");
        void Status(string message, [CallerMemberName] string memberName = "");
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
        private int PadLeft = 16;
        public void Error(string message, [CallerMemberName] string memberName = "")
        {
            var msg = $"[{memberName.PadLeft(PadLeft)}] -> {message}";
            _logFile.Error(msg);
        }
        public void Debug(string message, [CallerMemberName] string memberName = "")
        {
            _logFile.Debug($"[{memberName.PadLeft(PadLeft)}] -> {message}");
        }

        public void Info(string message, [CallerMemberName] string memberName = "")
        {
            _logFile.Info($"[{memberName.PadLeft(PadLeft)}] -> {message}");
        }

        public void Data(string message, [CallerMemberName] string memberName = "")
        {
            _logData.Info($"[{memberName.PadLeft(PadLeft)}] -> {message}");
        }

        public void Queries(string message, [CallerMemberName] string memberName = "")
        {
            _logQueries.Info($"[{memberName.PadLeft(PadLeft)}] -> {message}");
        }

        public void Status(string message, [CallerMemberName] string memberName = "")
        {
            _logStatus.Info($"[{memberName.PadLeft(PadLeft)}] -> {message}");
        }
    }


}