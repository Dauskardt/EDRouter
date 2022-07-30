using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class FSSSignalDiscoveredEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T13:08:35Z", "event":"FSSSignalDiscovered", "SystemAddress":2548425561450, "SignalName":"$Fixed_Event_Life_Cloud;", "SignalName_Localised":"Bemerkenswerte Sternenphänomene" }

        public FSSSignalDiscoveredEvent()
        {
            this.Event = "FSSSignalDiscovered";
        }

        public long SystemAddress { get; set; }
        public string SignalName { get; set; }
        public string SignalName_Localised { get; set; }

    }
}
