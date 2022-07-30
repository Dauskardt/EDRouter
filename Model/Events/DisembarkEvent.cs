using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class DisembarkEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T11:48:25Z", "event":"Disembark", "SRV":false, "Taxi":false, "Multicrew":false, "ID":304, "StarSystem":"NGC 6820 Sector JH-V c2-34", "SystemAddress":9420373235058, "Body":"NGC 6820 Sector JH-V c2-34 B 1", "BodyID":22, "OnStation":false, "OnPlanet":true }

        public DisembarkEvent()
        {
            this.Event = "Disembark";
        }

        public bool SRV { get; set; }
        public bool Taxi { get; set; }
        public bool Multicrew { get; set; }
        public int ID { get; set; }

        public string StarSystem { get; set; }
        public string Sector { get; set; }
        public long SystemAddress { get; set; }

        public string Body { get; set; }
        public int BodyID { get; set; }
        public bool OnStation { get; set; }
        public bool OnPlanet { get; set; }
    }
}
