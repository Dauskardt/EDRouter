using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class CommanderEvent:EventBase
    {
        public string FID { get; set; }
        public string Name { get; set; }

        public CommanderEvent()
        {
            Event = "Commander";
        }

        public override string ToString()
        {
            return Name;
        }

        //{ "timestamp":"2021-11-30T14:14:46Z", "event":"Commander", "FID":"F2979907", "Name":"SH4DOWM4K3R" }
    }
}
