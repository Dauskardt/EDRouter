using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class FSDTargetEvent:EventBase
    {
        //{ "timestamp":"2021-12-25T20:23:08Z", "event":"FSDTarget", "Name":"Aurai", "SystemAddress":7268024067513, "StarClass":"M", "RemainingJumpsInRoute":1 }

        public FSDTargetEvent() 
        {
            this.Event = "FSDTarget";
        }

        public string Name { get; set; }
        public long SystemAddress { get; set; }
        public string StarClass { get; set; }
        public int RemainingJumpsInRoute { get; set; }

    }
}
