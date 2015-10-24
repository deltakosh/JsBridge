using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChakraBridge
{
    public interface INode : IEventTarget
    {
        IDocument ownerDocument { get; }
    }
}
