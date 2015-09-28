using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Entities
{
    public delegate void ExecuteFunctionHandler();
    public delegate void PeopleFunctionHandler(People people);

    public sealed class DataManager
    {
        private static DataManager current;
        public static DataManager Current => current ?? (current = new DataManager());

        // When people list is ready
        private EventRegistrationTokenTable<EventHandler<IEnumerable<People>>> onPeopleReceived;

        public event EventHandler<IEnumerable<People>> OnPeopleReceived
        {
            add
            {
                return EventRegistrationTokenTable<EventHandler<IEnumerable<People>>>
                    .GetOrCreateEventRegistrationTokenTable(ref onPeopleReceived)
                    .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<IEnumerable<People>>>
                    .GetOrCreateEventRegistrationTokenTable(ref onPeopleReceived)
                    .RemoveEventHandler(value);
            }
        }

        public void raiseOnPeopleReceived(IEnumerable<People> people)
        {
            var temp = EventRegistrationTokenTable<EventHandler<IEnumerable<People>>>.GetOrCreateEventRegistrationTokenTable(ref onPeopleReceived).InvocationList;

            temp?.Invoke(null, people);
        }

        // Commit
        public ExecuteFunctionHandler commitFunction { get; set; }

        public void Commit()
        {
            commitFunction?.Invoke();
        }

        // Rollback
        public ExecuteFunctionHandler rollbackFunction { get; set; }
        public void Rollback()
        {
            rollbackFunction?.Invoke();
        }

        // Delete
        public PeopleFunctionHandler deleteFunction { get; set; }
        public void Delete(People people)
        {
            deleteFunction?.Invoke(people);
        }

    }
}
