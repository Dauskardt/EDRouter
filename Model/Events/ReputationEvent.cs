using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class ReputationEvent:EventBase
    {
        public double Empire { get; set; }
        public double Federation { get; set; }
        public double Alliance { get; set; }

        public ReputationEvent()
        {
            this.Event = "Reputation";
        }

        //{ "timestamp":"2021-12-06T09:29:21Z", "event":"Reputation", "Empire":99.235397, "Federation":99.235703, "Alliance":95.713203 }
    }
}
