using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace ChakraBridge
{
    class CanvasRenderingContext2D : ICanvasRenderingContext2D
    {
        private Window window;
        private Stack<DrawingState> savedStates;
        private DrawingState state;
        private float x0;
        private float y0;

        internal CanvasRenderingContext2D(Window window, IHTMLCanvasElement canvas)
        {
            this.window = window;
            this.canvas = canvas;

            this.savedStates = new Stack<DrawingState>();
            this.state = new DrawingState();
        }

        public IHTMLCanvasElement canvas { get; }

        public string fillStyle
        {
            get
            {
                return null;
            }

            set
            {
                this.state.Fill = ParseRgb(value);
            }
        }

        public float lineWidth
        {
            get { return this.state.LineWidth;}
            set { this.state.LineWidth = value; }
        }

        public string strokeStyle
        {
            get
            {
                return null;
            }

            set
            {
                this.state.Stroke = ParseRgb(value);
            }
        }

        public void beginPath()
        {
        }

        public void clearRect(float x, float y, float width, float height)
        {
            this.window.Session.Clear(Colors.Transparent);
        }

        public void closePath()
        {

        }

        public void fillRect(float x, float y, float width, float height)
        {
            this.window.Session.FillRectangle(x, y, width, height, this.state.Fill);
        }

        public IImageData getImageData(float sx, float sy, float sw, float sh)
        {
            return new ImageData {
                width = (int)sw,
                height = (int)sh
            };
        }

        public void lineTo(float x, float y)
        {
            this.window.Session.DrawLine(x0, y0, x, y, this.state.Stroke, this.state.LineWidth);

            x0 = x;
            y0 = y;
        }

        public void moveTo(float x, float y)
        {
            this.x0 = x;
            this.y0 = y;
        }

        public void restore()
        {
            if (this.savedStates.Any()) {
                this.state = this.savedStates.Pop();
                this.window.Session.Transform = this.state.Transform;
            }
        }

        public void save()
        {
            this.savedStates.Push(this.state.Clone());
        }

        public void stroke()
        {

        }

        public void transform(float a, float b, float c, float d, float e, float f)
        {
            this.state.Transform = new Matrix3x2(a, b, c, d, e, f);
            this.window.Session.Transform = this.state.Transform;
        }

        private static Color ParseRgb(string value)
        {
            if (value?.StartsWith("rgb(") ?? false) {
                var parts = value.Substring(4, value.Length - 5).Split(',');
                if (parts.Length == 3) {
                    return Color.FromArgb(0xff, ParseByte(parts[0]), ParseByte(parts[1]), ParseByte(parts[2]));
                }
            }
            return Colors.Black;
        }
        private static byte ParseByte(string value)
        {
            if (value == "NaN") {
                return 0;
            }
            return byte.Parse(value);
        }
    }
}
