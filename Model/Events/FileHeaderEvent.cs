using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class FileHeaderEvent: EventBase
    {
        public int Part { get; set; }
        public string language { get; set; }
        public bool Odyssey { get; set; }
        public string gameversion { get; set; }
        public string build { get; set; }

        public FileHeaderEvent()
        {
            Event = "FileHeader";
        }

        //{ "timestamp":"2021-11-30T14:13:01Z", "event":"Fileheader", "part":1, "language":"German/DE", "Odyssey":true, "gameversion":"4.0.0.903", "build":"r277609/r0 " }
    }
}
