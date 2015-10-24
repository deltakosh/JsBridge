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
        string fillStyle { get; set; }
        float lineWidth { get; set; }
        string strokeStyle { get; set; }
        void fillRect(float x, float y, float width, float height);
    }
}
