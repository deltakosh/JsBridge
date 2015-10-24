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
        void beginPath();
        void bezierCurveTo(float cp1x, float cp1y, float cp2x, float cp2y, float x, float y);
        void clearRect(float x, float y, float width, float height);
        void closePath();
        void fill();
        void fillRect(float x, float y, float width, float height);
        IImageData getImageData(float sx, float sy, float sw, float sh);
        void lineTo(float x, float y);
        void moveTo(float x, float y);
        void restore();
        void save();
        void stroke();
        void transform(float a, float b, float c, float d, float e, float f);
    }
}
