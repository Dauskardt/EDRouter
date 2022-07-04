using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class NavRouteEvent:EventBase
    {

        public List<Part> Route { get; set; }

        public NavRouteEvent()
        {
            this.Event = "NavRoute";
        }

        //{ "timestamp":"2021-12-24T08:45:55Z", "event":"NavRoute", "Route":[
        //{ "StarSystem":"Shinrarta Dezhra", "SystemAddress":3932277478106, "StarPos":[55.71875,17.59375,27.15625], "StarClass":"K" }, 
        //{ "StarSystem":"Tionisla", "SystemAddress":5031789105890, "StarPos":[82.25000,48.75000,68.15625], "StarClass":"K" }] }

        public string CRC { get { return Crc32.CRC32OfString(timestamp + Event + Route[0].StarSystem + Route[1].StarSystem); } }
    }

    [Serializable]
    public class Part
    {
       public string StarSystem { get; set; }
       public long SystemAddress { get; set; }
       public double[] StarPos { get; set; }
       public string StarClass { get; set; }

        public override string ToString()
        {
            return StarSystem;
        }
    }
}
