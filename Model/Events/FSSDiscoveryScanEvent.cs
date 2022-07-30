using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class FSSDiscoveryScanEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T13:08:35Z", "event":"FSSDiscoveryScan", "Progress":0.457531, "BodyCount":16, "NonBodyCount":7, "SystemName":"NGC 6820 Sector FB-X c1-9", "SystemAddress":2548425561450 }

        public FSSDiscoveryScanEvent()
        {
            this.Event = "FSSDiscoveryScan";
        }

        public double Progress { get; set; }
        public int BodyCount { get; set; }
        public int NonBodyCount { get; set; }
        public string SystemName { get; set; }
        public string Sector { get; set; }
        public long SystemAddress { get; set; }

    }
}
