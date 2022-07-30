using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class DockSRVEvent : EventBase
    {
        //"DockSRV", "SRVType":"testbuggy", "SRVType_Localised":"SRV Scarab", "ID":363 

        public string SRVType { get; set; }
        public string SRVType_Localised { get; set; }
        public string SRV { get; set; }
        public int ID { get; set; }

        public DockSRVEvent()
        {
            Event = "DockSRV";
        }

    }
}
