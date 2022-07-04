using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class StatusEvent:EventBase
    {

        public StatusEvent()
        {
            this.Event = "Status";
        }

       // { "timestamp":"2021-12-24T09:56:12Z", "event":"Status", "Flags":151060493, "Flags2":0, "Pips":[4,8,0], "FireGroup":0, "GuiFocus":0, "Fuel":{ "FuelMain":16.000000, "FuelReservoir":0.152118 }, "Cargo":0.000000, "LegalState":"Clean", "Balance":25038692802, "Destination":{ "System":5306667012834, "Body":38, "Name":"Sharon Lee Free Market" } }
        
        public long Flags { get; set; }
        public long Flags2 { get; set; }
        public int[] Pips { get; set; }
        public int FireGroup { get; set; }
        public int GuiFocus { get; set; }

        public FuelValues Fuel { get; set; }
        public double Cargo { get; set; }
        
        public string LegalState { get; set; }
        public long Balance { get; set; }

        public DestinationValues Destination { get; set; }

    }

    [Serializable]
    public class FuelValues
    {
       public double FuelMain { get; set; }
       public double FuelReservoir { get; set; }
    }

    [Serializable]
    public class DestinationValues
    {
        public long System { get; set; }
        public int Body { get; set; }
        public string Name { get; set; }
    }
}
