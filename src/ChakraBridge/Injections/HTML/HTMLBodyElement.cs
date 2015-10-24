using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    class HTMLBodyElement : HTMLElement, IHTMLBodyElement
    {
        internal HTMLBodyElement(IWindow window)
            : base(window)
        {
        }
    }
}
