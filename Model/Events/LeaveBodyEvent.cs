using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class LeaveBodyEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T11:07:20Z", "event":"LeaveBody", "StarSystem":"NGC 6820 Sector JH-V c2-34", "SystemAddress":9420373235058, "Body":"NGC 6820 Sector JH-V c2-34 A 7", "BodyID":16 }

        public LeaveBodyEvent()
        {
            this.Event = "LeaveBody";
        }

        public string StarSystem { get; set; }
        public string Sector { get; set; }
        public long SystemAddress { get; set; }
        public string Body { get; set; }
        public int BodyID { get; set; }

}
}
