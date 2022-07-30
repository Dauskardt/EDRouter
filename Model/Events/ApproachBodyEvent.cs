using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class ApproachBodyEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T11:36:08Z", "event":"ApproachBody", "StarSystem":"NGC 6820 Sector JH-V c2-34", "SystemAddress":9420373235058, "Body":"NGC 6820 Sector JH-V c2-34 B 1", "BodyID":22 }

        public ApproachBodyEvent()
        {
            this.Event = "ApproachBody";
        }

        public string StarSystem { get; set; }
        public string Sector { get; set; }
        public long SystemAddress { get; set; }
        public string Body { get; set; }
        public int BodyID { get; set; }

    }
}
