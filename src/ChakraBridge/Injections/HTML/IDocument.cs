using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    public interface IDocument : INode
    {
        IHTMLBodyElement body { get; }
        IWindow defaultView { get; }
        IElement documentElement { get; }
        object createElement(string tagName);
    }
}
