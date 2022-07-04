using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events

{
    [Serializable]
    public class MaterialsEvent : EventBase
    {
        public List<MaterialHorizons> Raw { get; set; }
        public List<MaterialHorizons> Manufactured { get; set; }
        public List<MaterialHorizons> Encoded { get; set; }

        public bool IsEmpty
        {
            get { return Raw == null && Manufactured == null && Encoded == null; }
        }

        public MaterialsEvent()
        {
            Event = "Materials";
        }
    }
    [Serializable]
    public class MaterialHorizons
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
