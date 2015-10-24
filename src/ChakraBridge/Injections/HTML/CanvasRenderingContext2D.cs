using System;
using System.Collections.Generic;
using System.Linq;
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

        public void fillRect(float x, float y, float width, float height)
        {
            this.window.Session.FillRectangle(x, y, width, height, this.state.Fill);
        }

        public void restore()
        {
            if (this.savedStates.Any()) {
                this.state = this.savedStates.Pop();
            }
        }

        public void save()
        {
            this.savedStates.Push(this.state.Clone());
        }

        private static Color ParseRgb(string value)
        {
            if (value?.StartsWith("rgb(") ?? false) {
                var parts = value.Substring(4, value.Length - 5).Split(',');
                if (parts.Length == 3) {
                    return Color.FromArgb(0xff, byte.Parse(parts[0]), byte.Parse(parts[1]), byte.Parse(parts[2]));
                }
            }
            return Colors.Black;
        }
    }
}
