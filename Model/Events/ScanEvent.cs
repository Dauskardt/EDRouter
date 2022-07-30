using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    public class ScanEvent:EventBase
    {
        //{ "timestamp":"2022-07-28T13:18:37Z", "event":"Scan", "ScanType":"Detailed", "BodyName":"NGC 6820 Sector FB-X c1-9 A 1", "BodyID":11,
        
        //"Parents":[ {"Star":1}, {"Null":0} ],
        
        //"StarSystem":"NGC 6820 Sector FB-X c1-9", "SystemAddress":2548425561450, "DistanceFromArrivalLS":61.868082,
        //"TidalLock":true, "TerraformState":"", "PlanetClass":"High metal content body", "Atmosphere":"",
        //"AtmosphereType":"None", "Volcanism":"", "MassEM":0.361541, "Radius":4462053.500000, "SurfaceGravity":7.237641,
        //"SurfaceTemperature":541.770142, "SurfacePressure":0.000000, "Landable":true,
        
        //"Materials":[ { "Name":"iron", "Name_Localised":"Eisen", "Percent":22.863373 }, { "Name":"nickel", "Percent":17.292896 }, { "Name":"sulphur", "Name_Localised":"Schwefel", "Percent":16.208258 }, { "Name":"carbon", "Name_Localised":"Kohlenstoff", "Percent":13.629466 }, { "Name":"manganese", "Name_Localised":"Mangan", "Percent":9.442341 }, { "Name":"phosphorus", "Name_Localised":"Phosphor", "Percent":8.725818 }, { "Name":"germanium", "Percent":4.761786 }, { "Name":"zirconium", "Percent":2.654909 }, { "Name":"niobium", "Name_Localised":"Niob", "Percent":1.562590 }, { "Name":"molybdenum", "Name_Localised":"Molibdän", "Percent":1.492965 }, { "Name":"yttrium", "Percent":1.365602 } ],
        
        //"Composition":{ "Ice":0.000000, "Rock":0.670341, "Metal":0.329659 },
        
        //"SemiMajorAxis":18548518419.265747, "Eccentricity":0.000477, "OrbitalInclination":0.468449, "Periapsis":61.261251, "OrbitalPeriod":1638408.184052, "AscendingNode":-162.289304, "MeanAnomaly":276.091040, "RotationPeriod":1618323.439645, "AxialTilt":0.349234,
        //"WasDiscovered":true, "WasMapped":true }

        public ScanEvent()
        {
            this.Event = "Scan";
        }

        public string ScanType { get; set; }
        public string BodyName { get; set; }
        public int BodyID { get; set; }

        public string StarSystem { get; set; }
        public long SystemAddress { get; set; }
        public double DistanceFromArrivalLS { get; set; }
        public bool TidalLock { get; set; }
        public string TerraformState { get; set; }
        public string PlanetClass { get; set; }
        public string Atmosphere { get; set; }
        public string AtmosphereType { get; set; }
        public string Volcanism { get; set; }
        public double MassEM { get; set; }
        public double Radius { get; set; }
        public double SurfaceGravity { get; set; }
        public double SurfaceTemperature { get; set; }
        public bool Landable { get; set; }

        public List<Material> Materials { get; set; }

        public double SemiMajorAxis { get; set; }
        public double Eccentricity { get; set; }
        public double OrbitalInclination { get; set; }
        public double Periapsis { get; set; }
        public double OrbitalPeriod { get; set; }
        public double AscendingNode { get; set; }
        public double MeanAnomaly { get; set; }
        public double RotationPeriod { get; set; }
        public double AxialTilt { get; set; }

        public bool WasDiscovered { get; set; }
        public bool WasMapped { get; set; }
    }

    //public class Parent
    //{
    //    public int Star { get; set; }
    //    public int Null { get; set; }
    //    public int Ring { get; set; }
    //    public int Ring { get; set; }
    //}

    [Serializable]
    public class Material
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public double Percent { get; set; }

    }
}
