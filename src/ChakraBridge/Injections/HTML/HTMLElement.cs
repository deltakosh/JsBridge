using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    abstract class HTMLElement : Element, IHTMLElement
    {
        internal HTMLElement(IWindow window)
            :base(window)
        {
        }

        public ICSSStyleDeclaration style { get; } = new CSSStyleDeclaration();
    }
}
