using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    class LaunchSRVEvent : EventBase
    {
        //"LaunchSRV", "SRVType":"testbuggy", "SRVType_Localised":"SRV Scarab", "Loadout":"starter", "ID":363, "PlayerControlled":true }

        public string SRVType { get; set; }
        public string SRVType_Localised { get; set; }
        public string Loadout { get; set; }
        public int ID { get; set; }
        public bool PlayerControlled { get; set; }

        public LaunchSRVEvent()
        {
            Event = "LaunchSRV";
        }

    }
}
