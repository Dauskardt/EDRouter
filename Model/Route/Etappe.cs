using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EDRouter.Model.Route
{
    [Serializable]
    public class Etappe: EtappeBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _System;
        [XmlAttribute]
        public string System 
        {
            get { return _System; }
            set { _System = value; RPCEvent(nameof(System)); } 
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _NS;
        public bool NS
        {
            get { return _NS; }
            set { _NS = value; RPCEvent(nameof(NS)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _Sprünge;
        public int Sprünge
        {
            get { return _Sprünge; }
            set { _Sprünge = value; RPCEvent(nameof(Sprünge)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float _Entfernung;
        public float Entfernung
        {
            get { return _Entfernung; }
            set { _Entfernung = value; RPCEvent(nameof(Entfernung)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float _Rest;
        public float Rest
        {
            get { return _Rest; }
            set { _Rest = value; RPCEvent(nameof(Rest)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _besucht;
        public bool besucht
        {
            get { return _besucht; }
            set { _besucht = value; RPCEvent(nameof(besucht)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime _TimeStamp;
        public DateTime TimeStamp
        {
            get { return _TimeStamp; }
            set { _TimeStamp = value; RPCEvent(nameof(TimeStamp)); }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _Info;
        public string Info
        {
            get { return _Info; }
            set { _Info = value; RPCEvent(nameof(Info)); }
        }

        public Etappe() { }

        public Etappe(string system, float Distance,float Remaining,bool NeutronStar, int Jumps) 
        {
            System = system;
            NS = NeutronStar;
            Sprünge = Jumps;
            Entfernung = Distance;
            Rest = Remaining;
        }

        public Etappe(string[] RawValues, Model.Route.ReiseRoute.RouteType route )
        {

            switch (route)
            {
                case Model.Route.ReiseRoute.RouteType.NR:
                    System = RawValues[0];
                    Entfernung = (float)Math.Round(Convert.ToSingle(RawValues[1].Replace('.', ',')), 2);
                    Rest = (float)Math.Round(Convert.ToSingle(RawValues[2].Replace('.', ',')), 2);
                    Sprünge = Convert.ToInt32(RawValues[4]);

                    switch (RawValues[3])
                    {
                        case "Yes":
                            NS = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case Model.Route.ReiseRoute.RouteType.FC:
                    //"System Name","Distance","Distance Remaining","Tritium in tank","Tritium in market","Fuel Used","Icy Ring","Pristine","Restock Tritium"
                    //"Sol","0","25899.9894653959","","5518","0","No","No","No"
                    System = RawValues[0];
                    Entfernung = (float)Math.Round(Convert.ToSingle(RawValues[1].Replace('.', ',')), 2);
                    Rest = (float)Math.Round(Convert.ToSingle(RawValues[2].Replace('.', ',')), 2);
                    Sprünge = 1;
                    Info = "Tritium-Verbrauch:" + (float)Math.Round(Convert.ToSingle(RawValues[5].Replace('.', ',')), 2) + " Tonnen";
                    break;
                case Model.Route.ReiseRoute.RouteType.TO:
                    break;
                case Model.Route.ReiseRoute.RouteType.TR:
                    break;
                case Model.Route.ReiseRoute.RouteType.RR:
                    System = RawValues[0];
                    Entfernung = (float)Math.Round(Convert.ToSingle(RawValues[4].Replace('.', ',')), 2);
                    Sprünge = Convert.ToInt32(RawValues[7]);
                    Info = RawValues[1].Replace(RawValues[0], string.Empty) + " - " + RawValues[2];
                    break;
                case Model.Route.ReiseRoute.RouteType.GY:
                    break;
                case Model.Route.ReiseRoute.RouteType.EL:
                    break;
                case Model.Route.ReiseRoute.RouteType.AM:
                    break;
                default:
                    break;
            }


        }

    }
}
