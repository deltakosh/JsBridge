using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ChakraBridge
{
    class DrawingState
    {
        public Matrix3x2 Transform { get; set; } = Matrix3x2.Identity;
        public Color Stroke { get; set; } = Colors.Black;
        public Color Fill { get; set; } = Colors.Black;
        public float LineWidth { get; set; } = 1;

        public DrawingState Clone()
        {
            return new DrawingState {
                Transform = Transform,
                Stroke = Stroke,
                Fill = Fill,
                LineWidth = LineWidth
            };
        }
    }
}
