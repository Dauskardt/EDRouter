using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDRouter.Model.Json
{
    [Serializable]
    public class JournalEvents:Model.ModelBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ObservableCollection<Model.Events.EventBase> _Events = new();
        public ObservableCollection<Model.Events.EventBase> Events
        {
            get { return _Events; }
            set { _Events = value; RPCEvent(nameof(Events)); }

        }

        public Model.Events.EventBase this[string Name] => Events.First(x => x.Event == Name);

        public void Clear()
        {
            Events.Clear();
        }
        
        public void Add(Model.Events.EventBase e)
        {
            Events.Add(e);
        }
        
        public void Remove(Model.Events.EventBase e)
        {
            Events.Remove(e);
        }
    }
}
