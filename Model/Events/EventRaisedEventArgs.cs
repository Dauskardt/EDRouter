using System;
using System.Collections.Generic;
using System.Text;

namespace EDRouter.Model.Events
{
    public class EventRaisedEventArgs : EventArgs
    {
        public string EventName { get; set; }
        public object EventObject { get; set; }
        public int Line { get; set; }

        public EventRaisedEventArgs(object Event, string name, int line)
        {
            EventObject = Event;
            EventName = name;
            Line = line;
        }
    }
}
