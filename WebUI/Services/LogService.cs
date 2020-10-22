using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
{
    public class LogService : ILogService
    {
        private HttpContext http { get; set; }

        public LogService(IHttpContextAccessor httpContextAccessor)
        {
            this.http = httpContextAccessor?.HttpContext;
        }


        private string RequestTraceIdentifier => http.TraceIdentifier;
        private string ClientIpAddress => http.GetClientIpAddress();
        private string CurrentUrl => http.GetCurrentUrl(addRequestMethod: true);


        private void Log(LogLevel level, string message, Exception exception = null, Guid? guid = null)
        {
            try
            {
                var sb = new System.Text.StringBuilder();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    sb.Append(message.Trim());
                }
                if (exception != null && !string.IsNullOrWhiteSpace(exception.StackTrace))
                {
                    sb.Append(" " + exception.StackTrace);
                }
                if (guid.HasValue)
                {
                    sb.Append(" " + "[[GUID:" + guid.Value.ToString("B").ToUpper() + "]]");
                }



                var _logModel = new Data.LogData
                {
                    LogLevel = level,
                    RequestTraceIdentifier = this.RequestTraceIdentifier,
                    ClientIpAddress = this.ClientIpAddress,
                    Message = sb.ToString(),
                    Url = CurrentUrl
                };

                _logModel.TrimStrings();

                ScheduledJobs.FireAndForgetJobs.SaveLog(_logModel);
            }
            catch (Exception)
            {

            }
        }


        public void LogError(string message, Exception exception = null)
        {
            this.Log(LogLevel.Error, message, exception);
        }

        public void LogDebug(string message, Exception exception = null, Guid? guid = null)
        {
            this.Log(LogLevel.Debug, message, exception, guid);
        }
        public void LogInformation(string message, Exception exception = null)
        {
            this.Log(LogLevel.Information, message, exception);
        }

        public void LogWarning(string message, Exception exception = null)
        {
            this.Log(LogLevel.Warning, message, exception);
        }

        public void LogTrace(string message, Exception exception = null)
        {
            this.Log(LogLevel.Trace, message, exception);
        }

        public void LogCritical(string message, Exception exception = null)
        {
            this.Log(LogLevel.Critical, message, exception);
        }
    }
}
