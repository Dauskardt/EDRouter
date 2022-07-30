using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class SAASignalsFoundEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T12:38:40Z", "event":"SAASignalsFound", "BodyName":"NGC 6820 Sector JH-V c2-34 B 4", "SystemAddress":9420373235058, "BodyID":25, "Signals":[ { "Type":"$SAA_SignalType_Biological;", "Type_Localised":"Biologisch", "Count":2 } ] }

        public SAASignalsFoundEvent()
        {
            this.Event = "SAASignalsFound";
        }

        public string BodyName { get; set; }
        public long SystemAddress { get; set; }
        public int BodyID { get; set; }

        public List<Signal> Signals { get; set; }
    }

    [Serializable]
    public class Signal
    {
        //"Signals":[ { "Type":"$SAA_SignalType_Biological;", "Type_Localised":"Biologisch", "Count":2 } ]

        public string Type { get; set; }
        public string Type_Localised { get; set; }
        public int Count { get; set; }
    }
}
