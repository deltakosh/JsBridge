using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    class Document : Node, IDocument
    {
        private HTMLCanvasElement canvas;

        internal Document(IWindow window)
            : base(window)
        {
            this.body = new HTMLBodyElement(window);
            this.defaultView = window;
            this.documentElement = new Element(window);
        }

        public IHTMLBodyElement body { get; }

        public IWindow defaultView { get; }

        public IElement documentElement { get; }

        public object createElement(string tagName)
        {
            if (tagName == "canvas") {
                if (this.canvas == null) {
                    this.canvas = new HTMLCanvasElement(this.ownerDocument.defaultView);
                }
                return this.canvas;
            }

            throw new NotSupportedException();   
        }
    }
}
