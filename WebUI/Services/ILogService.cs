using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
{
    public interface ILogService
    {
        void LogError(string message, Exception exception = null);
        void LogDebug(string message, Exception exception = null, Guid? guid = null);
        void LogInformation(string message, Exception exception = null);
        void LogWarning(string message, Exception exception = null);
        void LogTrace(string message, Exception exception = null);
        void LogCritical(string message, Exception exception = null);
    }
}
