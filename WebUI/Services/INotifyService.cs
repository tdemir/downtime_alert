using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
{
    public interface INotifyService
    {
        void Send(Data.TargetApp item, string destination);
    }
}
