using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoListWPF.Models
{
    public class EventSent
    {
    }

    public class DispatcherTimerWindowCloseEvent : PubSubEvent<bool> { }
}
