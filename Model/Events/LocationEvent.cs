using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class LocationEvent:EventBase
    {
        public double DistFromStarLS { get; set; }

        public bool Docked { get; set; }
        public bool OnFoot { get; set; }
        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public double[] StarPos { get; set; }
        public string SystemAllegiance{ get; set; }
        public string SystemEconomy { get; set; }
        public string SystemEconomy_Localised { get; set; }
        public string SystemSecondEconomy { get; set; }
        public string SystemSecondEconomy_Localised { get; set; }
        public string SystemGovernment { get; set; }
        public string SystemGovernment_Localised { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemSecurity_Localised { get; set; }
        public long Population { get; set; }
        public string Body { get; set; }
        public int BodyID { get; set; }
        public string BodyType { get; set; }


        public List<cFactions> Factions { get; set; }



        public LocationEvent()
        {
            this.Event = "Location";
        }

        [Serializable]
        public class cFactions
        {
            public string Name { get; set; }
            public string FactionState { get; set; }
            public string Government { get; set; }
            public double Influence { get; set; }
            public string Allegiance { get; set; }

            public string Happiness { get; set; }
            public string Happiness_Localised { get; set; }
            public double MyReputation { get; set; }

            public override string ToString()
            {
                return Name + " [" + Allegiance + "]";
            }
        }

        [Serializable]
        public class cConflicts
        {
            public string WarType { get; set; }
            public string Status { get; set; }


        }
    }
}
