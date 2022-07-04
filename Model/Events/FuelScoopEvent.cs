using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    public class FuelScoopEvent:EventBase
    {
        //{ "timestamp":"2021-12-25T09:06:30Z", "event":"FuelScoop", "Scooped":0.015121, "Total":16.000000 }

        public FuelScoopEvent() 
        {
            this.Event = "FuelScoop";
        }

        public double Scooped { get; set; }
        public double Total { get; set; }
    }
}
