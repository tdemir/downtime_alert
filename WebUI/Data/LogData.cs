using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Data
{
    public class LogData : BaseTable
    {
        public LogLevel LogLevel { get; set; }

        [MaxLength(4000)]
        public string Message { get; set; }
        [MaxLength(100)]
        public string RequestTraceIdentifier { get; set; }
        [MaxLength(100)]
        public string ClientIpAddress { get; set; }
        [MaxLength(500)]
        public string Url { get; set; }

        public void TrimStrings()
        {
            Func<string, int, string> _fnc = (str, lngth) =>
            {
                if (string.IsNullOrEmpty(str))
                {
                    return string.Empty;
                }
                str = str.Trim();
                if (str.Length > lngth)
                {
                    str = str.Substring(0, lngth);
                }
                return str;
            };

            Message = _fnc(Message, 4000);
            RequestTraceIdentifier = _fnc(RequestTraceIdentifier, 100);
            ClientIpAddress = _fnc(ClientIpAddress, 100);
            Url = _fnc(Url, 500);
        }
    }
}
