using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class SupercruiseExitEvent:EventBase
    {

        public SupercruiseExitEvent()
        {
            this.Event = "SupercruiseExit";
        }

        public bool Taxi { get; set; }
        public bool Multicrew { get; set; }
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public string Body { get; set; }
        public int BodyID { get; set; }
        public string BodyType { get; set; }

        //{ "timestamp":"2021-12-25T08:29:54Z", "event":"SupercruiseExit", "Taxi":false, "Multicrew":false, "StarSystem":"FN Virginis", "SystemAddress":20462163862953, "Body":"Jacobi Landing", "BodyID":65, "BodyType":"Station" }
    }
}
