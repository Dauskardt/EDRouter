using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class StartJumpEvent:EventBase
    {

        //{ "timestamp":"2021-12-25T09:13:53Z", "event":"StartJump", "JumpType":"Supercruise" }

        public StartJumpEvent()
        {
            this.Event = "StartJump";
        }

       public string JumpType { get; set; }
    }
}
