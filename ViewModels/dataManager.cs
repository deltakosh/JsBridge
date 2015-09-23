using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace ViewModels
{
    public delegate void ExecuteFunctionHandler();

    public sealed class DataManager
    {
        private static DataManager current;
        public static DataManager Current => current ?? (current = new DataManager());

        // When people list is ready
        private EventRegistrationTokenTable<EventHandler<People[]>> onPeopleReceived;

        public event EventHandler<People[]> OnPeopleReceived
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

        // Commit
        public ExecuteFunctionHandler commitFunction { get; set; }

        public void Commit()
        {
            commitFunction?.Invoke();
        }
    }
}
