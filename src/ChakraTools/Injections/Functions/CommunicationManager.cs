using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ChakraTools
{
    public delegate void ObjectReceivedHandler(object obj);

    public static class CommunicationManager
    {
        public static ObjectReceivedHandler OnObjectReceived { get; set; }

        static readonly Dictionary<string, Type> RegisteredTypes = new Dictionary<string, Type>();

        public static void RegisterType(Type type)
        {
            RegisteredTypes.Add(type.Name, type);
        }

        internal static JavaScriptValue SendToHostJavaScriptNativeFunction(JavaScriptValue callee, bool isConstructCall, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            // sendToHost signature is (json, type)
            JavaScriptValue jsonValue = arguments[1];
            string json = jsonValue.ConvertToString().ToString();

            JavaScriptValue typeValue = arguments[2];
            string typename = typeValue.ConvertToString().ToString();

            if (!RegisteredTypes.ContainsKey(typename))
            {
                throw new Exception("Not registered type found: " + typename);
            }

            var type = RegisteredTypes[typename];

            var poco = JsonConvert.DeserializeObject(json, type);

            OnObjectReceived?.Invoke(poco);

            return JavaScriptValue.True;
        }
    }
}
