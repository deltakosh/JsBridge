using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    class Window : EventTarget, IWindow
    {
        public Window()
        {
            this.document = new Document(this);
        }
        public IDocument document { get; }

        public ILocation location { get; } = new Location();

        public INavigator navigator { get; } = new Navigator();

        public string atob(string encodedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
        }

        public string btoa(string stringToEncode)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToEncode));
        }
    }
}
