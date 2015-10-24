using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    public interface IWindow : IEventTarget
    {
        IDocument document { get; }
        ILocation location { get; }
        INavigator navigator { get; }

        string btoa(string stringToEncode);
        string atob(string encodedData);

        object Render();
    }
}
