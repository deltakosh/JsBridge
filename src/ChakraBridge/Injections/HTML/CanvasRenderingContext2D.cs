using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    class CanvasRenderingContext2D : ICanvasRenderingContext2D
    {
        internal CanvasRenderingContext2D(IHTMLCanvasElement canvas)
        {
            this.canvas = canvas;
        }

        public IHTMLCanvasElement canvas { get; }

        public object fillStyle
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        public void fillRect(float x, float y, float width, float height)
        {
        }
    }
}
