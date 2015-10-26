using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    /// <summary>
    /// Wraps HTMLCanvasElement and implements IHTMLCanvasElement, required to make instanceof HTMLCanvasElement work in javascript
    /// </summary>
    public sealed class HTMLCanvasElementWrapper : IHTMLCanvasElement
    {
        private HTMLCanvasElement canvas;

        internal HTMLCanvasElementWrapper(HTMLCanvasElement canvas)
        {
            this.canvas = canvas;
        }

        public int clientHeight
        {
            get
            {
                return this.canvas.clientHeight;
            }
        }

        public int clientLeft
        {
            get
            {
                return this.canvas.clientLeft;
            }
        }

        public int clientTop
        {
            get
            {
                return this.canvas.clientTop;
            }
        }

        public int clientWidth
        {
            get
            {
                return this.canvas.clientWidth;
            }
        }

        public int height
        {
            get
            {
                return this.canvas.height;
            }
        }

        public IDocument ownerDocument
        {
            get
            {
                return this.canvas.ownerDocument;
            }
        }

        public ICSSStyleDeclaration style
        {
            get
            {
                return this.canvas.style;
            }
        }

        public int width
        {
            get
            {
                return this.canvas.width;
            }
        }

        public void addEventListener()
        {
            this.canvas.addEventListener();
        }

        public string getAttribute(string attributeName)
        {
            return this.canvas.getAttribute(attributeName);
        }

        public object getContext(string contextType)
        {
            return this.canvas.getContext(contextType);
        }

        public bool hasAttribute(string attName)
        {
            return this.canvas.hasAttribute(attName);
        }

        public void setAttribute(string name, string value)
        {
            this.canvas.setAttribute(name, value);
        }
    }
}
