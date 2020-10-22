using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ScheduledJobs
{
    public class FireAndForgetJobs
    {
        [AutomaticRetry(Attempts = 3)]
        public static void SaveLog(Data.LogData item)
        {
            BackgroundJob.Enqueue<Managers.LogManager>(x => x.Process(item));
        }
    }
}
