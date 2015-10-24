using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    public interface IImageData
    {
        object data { get; }
        int height { get; }
        int width { get; }
    }
}
