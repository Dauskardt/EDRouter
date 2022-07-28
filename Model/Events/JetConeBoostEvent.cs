using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
   public class JetConeBoostEvent : EventBase
   {
        //{ "timestamp":"2022-07-27T06:21:02Z", "event":"JetConeBoost", "BoostValue":4.000000 }

        public JetConeBoostEvent()
        {
            this.Event = "JetConeBoost";
        }

        public double BoostValue { get; set; }
   }
}
