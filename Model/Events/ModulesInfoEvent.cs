using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class ModulesInfoEvent : EventBase
    {
        public List<ModuleObject> Modules { get; set; }

        public ModulesInfoEvent()
        {
            this.Event = "ModulesInfo";
        }
    }

    public class ModuleObject
    { 
        //"Slot":"MainEngines", "Item":"int_engine_size6_class5", "Power":8.467200, "Priority":0
        public string Slot { get; set; }
        public string Item { get; set; }
        public double Power { get; set; }
        public int Priority { get; set; }

        public override string ToString()
        {
            return Slot + " [Power:" + Power + " Priority:" + Priority + "]";
        }
    }
}
