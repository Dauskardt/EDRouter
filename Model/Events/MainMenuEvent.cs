using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class MainMenuEvent:EventBase
    {
        public MainMenuEvent() 
        {
            this.Event = "MainMenue";
        }

        public MainMenuEvent(DateTime dt)
        {
            this.Event = "MainMenue";
            this.timestamp = dt;
        }
    }
}
