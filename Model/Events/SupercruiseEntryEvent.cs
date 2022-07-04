using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class SupercruiseEntryEvent:EventBase
    {
        //{ "timestamp":"2021-12-25T10:14:48Z", "event":"SupercruiseEntry", "Taxi":false, "Multicrew":false, "StarSystem":"FN Virginis", "SystemAddress":20462163862953 }

        public SupercruiseEntryEvent()
        {
            this.Event = "SupercruiseEntry";
        }

        public bool Taxi { get; set; }
        public bool Multicrew { get; set; }
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
    }
}
