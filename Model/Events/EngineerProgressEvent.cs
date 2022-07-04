using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Events
{
    [Serializable]
    public class EngineerProgressEvent : EventBase
    {
        public List<EngineerObject> Engineers { get; set; }

        public EngineerProgressEvent()
        {
            Event = "EngineerProgress";
        }

        /*
        { "timestamp":"2021-11-30T14:14:46Z", "event":"EngineerProgress", "Engineers":[ { "Engineer":"Hera Tani", "EngineerID":300090, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"The Sarge", "EngineerID":300040, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Professor Palin", "EngineerID":300220, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Felicity Farseer", "EngineerID":300100, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Eleanor Bresa", "EngineerID":400011, "Progress":"Known" }, { "Engineer":"Hero Ferrari", "EngineerID":400003, "Progress":"Known" }, { "Engineer":"Tiana Fortune", "EngineerID":300270, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Jude Navarro", "EngineerID":400001, "Progress":"Unlocked", "RankProgress":0, "Rank":0 }, { "Engineer":"Broo Tarquin", "EngineerID":300030, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Etienne Dorn", "EngineerID":300290, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Lori Jameson", "EngineerID":300230, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Bill Turner", "EngineerID":300010, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Liz Ryder", "EngineerID":300080, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Rosa Dayette", "EngineerID":400012, "Progress":"Known" }, { "Engineer":"Juri Ishmaak", "EngineerID":300250, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Zacariah Nemo", "EngineerID":300050, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Mel Brandon", "EngineerID":300280, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Selene Jean", "EngineerID":300210, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Marco Qwent", "EngineerID":300200, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Chloe Sedesi", "EngineerID":300300, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Baltanos", "EngineerID":400010, "Progress":"Known" }, { "Engineer":"Petra Olmanova", "EngineerID":300130, "Progress":"Invited" }, { "Engineer":"Ram Tah", "EngineerID":300110, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"The Dweller", "EngineerID":300180, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Elvira Martuuk", "EngineerID":300160, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Terra Velasquez", "EngineerID":400006, "Progress":"Known" }, { "Engineer":"Lei Cheung", "EngineerID":300120, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Kit Fowler", "EngineerID":400004, "Progress":"Known" }, { "Engineer":"Colonel Bris Dekker", "EngineerID":300140, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Didi Vatermann", "EngineerID":300000, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Tod 'The Blaster' McQuinn", "EngineerID":300260, "Progress":"Unlocked", "RankProgress":0, "Rank":5 }, { "Engineer":"Domino Green", "EngineerID":400002, "Progress":"Unlocked", "RankProgress":0, "Rank":0 }, { "Engineer":"Marsha Hicks", "EngineerID":300150, "Progress":"Invited" } ] }


        */
    }

    public class EngineerObject: IComparable<EngineerObject>
    {
        public string Engineer { get; set; }
        public int EngineerID { get; set; }
        public string Progress { get; set; }
        public int RankProgress { get; set; }
        public int Rank { get; set; }

        public int CompareTo(EngineerObject other)
        {
            return EngineerID.CompareTo(other.EngineerID);
        }

        public override string ToString()
        {
            return EngineerID + " " + Engineer + " [" + Progress + "]";
        }

    }
}
