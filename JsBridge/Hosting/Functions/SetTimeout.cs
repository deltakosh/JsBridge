using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using ChakraHost.Hosting;

namespace JSBridge.Hosting.Functions
{
    public static class SetTimeout
    {
        public static JavaScriptValue SetTimeoutJavaScriptNativeFunction(JavaScriptValue callee, bool isConstructCall, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            // setTimeout signature is (callback, after)
            JavaScriptValue callbackValue = arguments[1];

            JavaScriptValue afterValue = arguments[2].ConvertToNumber();
            var after = Math.Max(afterValue.ToDouble(), 1);

            uint refCount;
            Native.JsAddRef(callbackValue, out refCount);
            Native.JsAddRef(callee, out refCount);

            ExecuteAsync((int)after, callbackValue, callee);

            return JavaScriptValue.True;
        }

        static async void ExecuteAsync(int delay, JavaScriptValue callbackValue, JavaScriptValue callee)
        {
            await Task.Delay(delay);
            callbackValue.CallFunction(callee);
            uint refCount;
            Native.JsRelease(callbackValue, out refCount);
            Native.JsRelease(callee, out refCount);
        }
    }
}
