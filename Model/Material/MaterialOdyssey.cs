using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Material
{
    [Serializable]
    public class MaterialOdyssey : Model.ModelBase, IComparable<MaterialOdyssey>
    {
        public string Name { get; set; }
        public string Name_Localised { get; set; }
        public int OwnerID { get; set; }
        public int MissionID { get; set; }
        public int Count { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return Name_Localised + " " + Name + " [" + Count + "] " + Type;
        }

        int IComparable<MaterialOdyssey>.CompareTo(MaterialOdyssey other)
        {
            return Name_Localised.CompareTo(other.Name_Localised);
        }

    }
}
