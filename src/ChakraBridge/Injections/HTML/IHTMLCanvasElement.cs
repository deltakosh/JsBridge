using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    public interface IHTMLCanvasElement : IHTMLElement
    {
        int width { get; }
        int height { get; }
        object getContext(string contextType);
    }
}
