using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class EventBus
    {
        private static Lazy<EventAggregator> eventAggregator = new Lazy<EventAggregator>();
        public static EventAggregator EventAggregatorInstance
        {
            get { return eventAggregator.Value; }
        }
    }
}
