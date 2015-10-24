using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    class Element : Node, IElement
    {
        internal Element(IWindow window)
            : base(window)
        {
        }

        public int clientHeight { get; private set; } = 500;
        public int clientLeft { get; private set; }

        public int clientTop { get; private set; }
        public int clientWidth { get; private set; } = 500;

        public string getAttribute(string attributeName)
        {
            return string.Empty;
        }

        public bool hasAttribute(string attName)
        {
            return false;
        }

        public void setAttribute(string name, string value)
        {
        }
    }
}
