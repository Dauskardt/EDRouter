using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    public class AfmuRepairsEvent:EventBase
    {

        public AfmuRepairsEvent()
        {
            this.Event = "AfmuRepairs";
        }

        public string Module { get; set; }
        public string Module_Localised { get; set; }
        public bool FullyRepaired { get; set; }
        public double Health { get; set; }

        //{ "timestamp":"2022-07-27T09:22:28Z", "event":"AfmuRepairs", "Module":"$int_hyperdrive_size5_class5_name;", "Module_Localised":"FSA", "FullyRepaired":true, "Health":1.000000 }

    }
}
