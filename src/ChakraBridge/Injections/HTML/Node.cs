using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    abstract class Node : EventTarget, INode
    {
        private IWindow window;

        internal Node(IWindow window)
        {
            this.window = window;
        }

        public IDocument ownerDocument
        {
            get
            {
                return this.window.document;
            }
        }
    }
}
