using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class TouchdownEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T11:04:39Z", "event":"Touchdown", "PlayerControlled":true, "Taxi":false, "Multicrew":false, "StarSystem":"NGC 6820 Sector JH-V c2-34", "SystemAddress":9420373235058, "Body":"NGC 6820 Sector JH-V c2-34 A 7", "BodyID":16, "OnStation":false, "OnPlanet":true, "Latitude":-39.506237, "Longitude":-112.601486 }

        public TouchdownEvent() 
        {
            this.Event = "Touchdown";
        }

        public bool PlayerControlled { get; set; }
        public bool Taxi { get; set; }
        public bool Multicrew { get; set; }
        public string StarSystem { get; set; }
        public string Sector { get; set; }
        public long SystemAddress { get; set; }

        public string Body { get; set; }
        public int BodyID { get; set; }
        public bool OnStation { get; set; }
        public bool OnPlanet { get; set; }

        public double Latitude { get; set; }
        public double Longitude{ get; set; }

    }
}
