using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.ScheduledJobs
{
    public static class RecurringJobs
    {
        [AutomaticRetry(Attempts = 3)]
        public static void AddJob(Data.TargetApp item, string mailAddress)
        {
            RecurringJob.RemoveIfExists(item.JobId);

            RecurringJob.AddOrUpdate<Managers.DowntimeCheckerManager>(item.JobId, 
                                                                      job => job.Process(item, mailAddress), 
                                                                      item.Interval, 
                                                                      TimeZoneInfo.Local);
        }

        [AutomaticRetry(Attempts = 3)]
        public static void RemoveJob(Data.TargetApp item)
        {
            RecurringJob.RemoveIfExists(item.JobId);
        }
    }
}
