using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    public class ShutdownEvent : EventBase
    {
        //{ "timestamp":"2022-07-25T06:51:22Z", "event":"Shutdown" }

        public ShutdownEvent()
        {
            this.Event = "Shutdown";
        }
    }
}
