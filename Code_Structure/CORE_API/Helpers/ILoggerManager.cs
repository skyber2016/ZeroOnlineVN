using System;

namespace CORE_API.Helpers
{
    public interface ILoggerManager
    {
        void Info(string message);
        void Error(string message);
        string GetLoggerId();
    }
}
