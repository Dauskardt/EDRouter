using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model
{
    [Serializable]
    public class Settings:Model.ModelBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _WindowWidth = 1000.0;
        public double WindowWidth
        {
            get { return _WindowWidth; }
            set { _WindowWidth = value; RPCEvent(nameof(WindowWidth)); }// Debug.Print(WindowWidth.ToString()); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _WindowHeigth = 620.0;
        public double WindowHeigth
        {
            get { return _WindowHeigth; }
            set { _WindowHeigth = value; RPCEvent(nameof(WindowHeigth)); }// Debug.Print(WindowHeigth.ToString()); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _WindowTop = 250.0;
        public double WindowTop
        {
            get { return _WindowTop; }
            set { _WindowTop = value; RPCEvent(nameof(WindowTop)); }// Debug.Print(WindowTop.ToString()); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _WindowLeft = 250.0;
        public double WindowLeft
        {
            get { return _WindowLeft; }
            set { _WindowLeft = value; RPCEvent(nameof(WindowLeft)); }// Debug.Print(WindowLeft.ToString()); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _LastRoute;
        public string LastRoute 
        {
            get { return _LastRoute; }
            set { _LastRoute = value; RPCEvent(nameof(LastRoute)); }
        
        }
    }
}
