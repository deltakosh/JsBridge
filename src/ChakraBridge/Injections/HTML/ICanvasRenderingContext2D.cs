using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    public interface ICanvasRenderingContext2D
    {
        IHTMLCanvasElement canvas { get; }
        object fillStyle { get; set; }
        void fillRect(float x, float y, float width, float height);
    }
}
