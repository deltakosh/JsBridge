using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
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
        private CanvasPathBuilder pathBuilder;

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
            get { return this.state.LineWidth; }
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
            if (this.pathBuilder != null) {
                closePath();
            }

            this.pathBuilder = new CanvasPathBuilder(CanvasDevice.GetSharedDevice());
            
        }

        public void bezierCurveTo(float cp1x, float cp1y, float cp2x, float cp2y, float x, float y)
        {
            if (this.pathBuilder == null) {
                beginPath();
            }
            this.pathBuilder.AddCubicBezier(new Vector2(cp1x, cp2y), new Vector2(cp2x, cp2y), new Vector2(x, y));
        }

        public void clearRect(float x, float y, float width, float height)
        {
            this.window.Session.Clear(Colors.Transparent);
        }

        public void closePath()
        {
            if (this.pathBuilder != null) {
                this.pathBuilder.Dispose();
                this.pathBuilder = null;
            }
        }

        public void fill()
        {
            if (this.pathBuilder != null) {
                this.pathBuilder.EndFigure(CanvasFigureLoop.Closed);
                var geometry = CanvasGeometry.CreatePath(this.pathBuilder);
                this.window.Session.Transform = this.state.Transform;
                this.window.Session.FillGeometry(geometry, this.state.Fill);

                closePath();
            }
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

            this.x0 = x;
            this.y0 = y;
        }

        public void moveTo(float x, float y)
        {
            if (this.pathBuilder != null) {
                this.pathBuilder.BeginFigure(x, y);
            }
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
            if (this.pathBuilder != null) {
                this.pathBuilder.EndFigure(CanvasFigureLoop.Open);
                var geometry = CanvasGeometry.CreatePath(this.pathBuilder);

                this.window.Session.Transform = this.state.Transform;
                this.window.Session.DrawGeometry(geometry, this.state.Stroke, this.state.LineWidth);

                closePath();
            }
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
