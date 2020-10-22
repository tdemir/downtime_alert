using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Data;

namespace WebUI.Services
{
    public interface ITargetAppService
    {
        void Add(TargetApp item);

        TargetApp Get(Guid id);

        IQueryable<TargetApp> List();

        void Update(TargetApp item);

        void Delete(Guid id);
    }
}
