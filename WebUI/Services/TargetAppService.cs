using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data;

namespace WebUI.Services
{
    public class TargetAppService : ITargetAppService
    {
        private readonly ApplicationDbContext db;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TargetAppService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            this.db = db;
            this.httpContextAccessor = httpContextAccessor;
        }

        private string CurrentUserId
        {
            get
            {
                return httpContextAccessor.HttpContext.User.Identity.GetId();
            }
        }

        private string CurrentUserMailAddress
        {
            get
            {
                return httpContextAccessor.HttpContext.User.Identity.Name;
            }
        }

        public void Add(TargetApp item)
        {
            item.UserId = CurrentUserId;
            db.Add(item);
            db.SaveChanges();

            if (item.Status)
            {
                ScheduledJobs.RecurringJobs.AddJob(item, CurrentUserMailAddress);
            }
        }


        public TargetApp Get(Guid id)
        {
            return db.TargetApps.FirstOrDefault(x => x.Id == id && x.UserId == CurrentUserId);
        }

        public IQueryable<TargetApp> List()
        {
            return db.TargetApps.Where(x => x.UserId == CurrentUserId);
        }

        public void Update(TargetApp item)
        {
            var x = Get(item.Id);
            if (x != null)
            {                
                x.Interval = item.Interval;
                x.Name = item.Name;
                x.Url = item.Url;
                x.Status = item.Status;                

                db.SaveChanges();

                ScheduledJobs.RecurringJobs.RemoveJob(x);
                if (x.Status)
                {
                    ScheduledJobs.RecurringJobs.AddJob(item, CurrentUserMailAddress);
                }
            }
        }

        public void Delete(Guid id)
        {
            var x = Get(id);
            if (x != null)
            {
                db.Remove(x);
                db.SaveChanges();

                ScheduledJobs.RecurringJobs.RemoveJob(x);
            }
        }
    }
}
