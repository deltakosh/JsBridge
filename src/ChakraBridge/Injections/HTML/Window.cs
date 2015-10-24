using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    class Window : EventTarget, IWindow
    {
        private CanvasRenderTarget target;
        private CanvasDrawingSession session;

        public Window()
        {
            this.document = new Document(this);
            this.target = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), 500, 500, 96);
        }
        public IDocument document { get; }

        public ILocation location { get; } = new Location();

        public INavigator navigator { get; } = new Navigator();

        public string atob(string encodedData)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(encodedData));
        }

        public string btoa(string stringToEncode)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(stringToEncode));
        }

        public object Render()
        {
            if (this.session != null) {
                this.session.Dispose();
                this.session = null;
            }
            return this.target;
        }

        internal CanvasDrawingSession Session
        {
            get
            {
                if (this.session == null) {
                    this.session = this.target.CreateDrawingSession();
                }
                return this.session;
            }
        }
    }
}
