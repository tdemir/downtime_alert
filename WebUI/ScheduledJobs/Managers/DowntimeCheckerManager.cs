using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Services;

namespace WebUI.ScheduledJobs.Managers
{
    public class DowntimeCheckerManager
    {
        INotifyService notifyService;
        public DowntimeCheckerManager(INotifyService notifyService)
        {
            this.notifyService = notifyService;
        }

        public async Task Process(Data.TargetApp item, string mailAddress)
        {

            var result = await CheckWebSite(item.Url);
            if (!result)
            {
                notifyService.Send(item, mailAddress);
            }

        }

        private async Task<bool> CheckWebSite(string _url)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                var result = await client.GetAsync(_url);
                int StatusCode = (int)result.StatusCode;

                return StatusCode >= 200 && StatusCode < 300;
            }
            catch (Exception)
            {

            }
            return false;
        }
    }
}
