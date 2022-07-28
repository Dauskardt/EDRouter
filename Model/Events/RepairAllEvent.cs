using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    public class RepairAllEvent:EventBase
    {

        public RepairAllEvent()
        {
            this.Event = "RepairAll";
        }

        public int Cost { get; set; }

    }
}
