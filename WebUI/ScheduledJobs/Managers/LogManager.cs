using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data;

namespace WebUI.ScheduledJobs.Managers
{
    public class LogManager
    {
        private ApplicationDbContext db;
        public LogManager(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task Process(Data.LogData item)
        {

            try
            {
                using (db)
                {
                    db.LogDatas.Add(item);
                    await db.SaveChangesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
