using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class LoadGameEvent:EventBase
    {

        //{ "timestamp":"2021-12-28T15:27:30Z", "event":"LoadGame", "FID":"F2979907", "Commander":"SH4DOWM4K3R", "Horizons":true, "Odyssey":true, "Ship":"Viper_MkIV", "Ship_Localised":"Viper Mk IV", "ShipID":355, "ShipName":"CREEPER", "ShipIdent":"SH4-07", "FuelLevel":16.000000, "FuelCapacity":16.000000, "GameMode":"Group", "Group":"Elitedangerous.de", "Credits":25083445013, "Loan":0, "language":"German/DE", "gameversion":"4.0.0.1002", "build":"r279380/r0 " }


        public string FID { get; set; }
        public string Commander { get; set; }
        public bool Horizons { get; set; }
        public bool Odyssey { get; set; }
        public string Ship { get; set; }
        public string Ship_Localised { get; set; }
        public int ShipID { get; set; }
        public string ShipName { get; set; }
        public string ShipIdent { get; set; }
        public double FuelLevel { get; set; }
        public double FuelCapacity { get; set; }
        public string GameMode { get; set; }
        public string Group { get; set; }
        public long Credits { get; set; }
        public double Loan { get; set; }
        public string language { get; set; }
        public string gameversion { get; set; }
        public string build { get; set; }

        public List<MaterialHorizonsBase> Raw { get; set; }
        public List<MaterialHorizonsBase> Manufactured { get; set; }
        public List<MaterialHorizonsBase> Encoded { get; set; }

        public bool IsEmpty
        {
            get { return Raw == null && Manufactured == null && Encoded == null; }
        }

        public LoadGameEvent()
        {
            Event = "LoadGame";
        }
    }

    public class MaterialHorizonsBase
    { 
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return Name + " [" + Count + "]";
        }

    }
}
