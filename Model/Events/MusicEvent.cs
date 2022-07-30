using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class MusicEvent:EventBase
    {

        public MusicEvent()
        {
            this.Event = "Music";
        }

        public string MusicTrack { get; set; }
    }
}
