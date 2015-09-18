using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace ViewModels
{
    public sealed class peopleManager
    {
        private static EventRegistrationTokenTable<EventHandler<People[]>> onPeopleReceived;

        public static event EventHandler<People[]> OnPeopleReceived
        {
            add
            {
                return EventRegistrationTokenTable<EventHandler<People[]>>
                    .GetOrCreateEventRegistrationTokenTable(ref onPeopleReceived)
                    .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<People[]>>
                    .GetOrCreateEventRegistrationTokenTable(ref onPeopleReceived)
                    .RemoveEventHandler(value);
            }
        }

        public void raiseOnPeopleReceived([ReadOnlyArray]People[] people)
        {
            EventHandler<People[]> temp = EventRegistrationTokenTable<EventHandler<People[]>>.GetOrCreateEventRegistrationTokenTable(ref onPeopleReceived).InvocationList;

            temp?.Invoke(null, people);
        }
    }
}
