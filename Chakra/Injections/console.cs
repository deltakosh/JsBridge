using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Chakra
{
    public sealed class Console
    {
        private static EventRegistrationTokenTable<EventHandler<string>> onLog;

        public static event EventHandler<string> OnLog
        {
            add
            {
                return EventRegistrationTokenTable<EventHandler<string>>
                    .GetOrCreateEventRegistrationTokenTable(ref onLog)
                    .AddEventHandler(value);
            }
            remove
            {
                EventRegistrationTokenTable<EventHandler<string>>
                    .GetOrCreateEventRegistrationTokenTable(ref onLog)
                    .RemoveEventHandler(value);
            }
        }

        public void log(string text)
        {
            Debug.WriteLine(text);

            EventHandler<string> temp = EventRegistrationTokenTable<EventHandler<string>>.GetOrCreateEventRegistrationTokenTable(ref onLog).InvocationList;

            temp?.Invoke(null, text);
        }
    }
}
