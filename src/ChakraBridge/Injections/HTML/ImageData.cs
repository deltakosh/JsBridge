using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    class ImageData : IImageData
    {
        public ImageData()
        {
            // create a js array
            JavaScriptValue reference;
            Native.ThrowIfError(Native.JsCreateArray(100, out reference));
            this.data = reference;
        }

        public object data { get; }

        public int height { get; internal set; }

        public int width { get; internal set; }
    }
}
