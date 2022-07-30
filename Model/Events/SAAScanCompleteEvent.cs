using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class SAAScanCompleteEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T12:38:40Z", "event":"SAAScanComplete", "BodyName":"NGC 6820 Sector JH-V c2-34 B 4", "SystemAddress":9420373235058, "BodyID":25, "ProbesUsed":3, "EfficiencyTarget":6 }

        public SAAScanCompleteEvent()
        {
            this.Event = "SAAScanComplete";
        }

        public string BodyName { get; set; }
        public string Sector { get; set; }
        public long SystemAddress { get; set; }
        public int BodyID { get; set; }
        public int ProbesUsed { get; set; }
        public int EfficiencyTarget { get; set; }

    }
}
