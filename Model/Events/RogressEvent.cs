using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class ProgressEvent:RankEvent
    {
        public ProgressEvent()
        {
            this.Event = "Progress";
        
        }
    }
}
