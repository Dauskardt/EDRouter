using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EDRouter.Model.Route
{
    [Serializable][XmlRoot("ReiseRoute")]
    public class ReiseRoute:ObservableCollection<Etappe>
    {

        public enum RouteType { NR, FC, TO, TR, RR, GY, EL, AM }

        public string GetRouteName { get { return "ROUTE " + this.Type + " [" + this[0].System.Replace("*",string.Empty) + "]-[" + this[this.Count-1].System.Replace("*", string.Empty) + "]"; } }

        public Etappe NextTarget { get { return  this.FirstOrDefault(x=> x.besucht == false); } }

        public Etappe ContainsEtappe(string SystemName) { return this.FirstOrDefault(x=> x.System == SystemName); }

        public RouteType Type { get; set; }

        public bool SetEtappeBesucht(string SystemAktuell)
        {
            Model.Route.Etappe EtappeErreicht = this.ContainsEtappe(SystemAktuell);

            if (EtappeErreicht != null)
            {
                EtappeErreicht.besucht = true;
                EtappeErreicht.TimeStamp = DateTime.Now;

                int IndexOfEtappe = this.IndexOf(EtappeErreicht);

                for (int i = 0; i < IndexOfEtappe; i++)
                {
                    if (!this[i].besucht)
                    {
                        this[i].besucht = true;
                        this[i].TimeStamp = DateTime.Now;
                    }
                }

                //TODO: Alle Vorgänger prüfen!
                return true;
            }

            return false;
        }

        public void Reset()
        {
            foreach (Etappe item in this)
            {
                item.TimeStamp = new DateTime();
                item.besucht = false;
            }
        }

        public bool Abgeschlossen()
        {
            return this.FirstOrDefault(x => x.besucht == false) == null;
        }

        public string SystemStandort
        {
            get 
            {
                Etappe Next = NextTarget;

                if (Next != null)
                {
                    return this[this.IndexOf(Next) - 1].System;
                }
                else
                {
                    return string.Empty;
                }
            
            }
        }


        public void SetNext()
        {
            Etappe Next = NextTarget;

            if (Next != null)
            {
                int Index = this.IndexOf(Next);

                if (Index != this.Count - 1)
                {
                    this[Index].besucht = true;
                    this[Index].TimeStamp = DateTime.Now;
                }
            }
        }

        public void SetPrevious()
        {
            Etappe Next = NextTarget;

            if (Next != null)
            {
                int Index = this.IndexOf(Next);

                if (Index != 0)
                {
                    this[Index - 1].besucht = false;
                    //this[Index - 1].TimeStamp = new DateTime();
                }
                else
                {
                    this[0].besucht = false;
                    //this[0].TimeStamp = new DateTime();
                }
            }
        }

        //var firstElement = lstComp.First().ComponentValue("Dep");

    }
}
