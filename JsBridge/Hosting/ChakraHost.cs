using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Storage;
using ChakraHost.Hosting;
using JSBridge;

namespace ChakraHost
{
    public class ChakraHost
    {
        private static JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        private static JavaScriptRuntime runtime;
        private static JavaScriptValue promiseCallback;

        public async Task<string> InitAsync()
        {
            JavaScriptContext context;

            if (Native.JsCreateRuntime(JavaScriptRuntimeAttributes.None, null, out runtime) != JavaScriptErrorCode.NoError)
                return "failed to create runtime.";

            if (Native.JsCreateContext(runtime, out context) != JavaScriptErrorCode.NoError)
                return "failed to create execution context.";

            if (Native.JsSetCurrentContext(context) != JavaScriptErrorCode.NoError)
                return "failed to set current context.";

            // ES6 Promise callback
            JavaScriptPromiseContinuationCallback promiseContinuationCallback = delegate (JavaScriptValue task, IntPtr callbackState)
            {
                promiseCallback = task;
            };

            if (Native.JsSetPromiseContinuationCallback(promiseContinuationCallback, IntPtr.Zero) != JavaScriptErrorCode.NoError)
                return "failed to setup callback for ES6 Promise";

            // Projections
            if (Native.JsProjectWinRTNamespace("Windows") != JavaScriptErrorCode.NoError)
                return "failed to project windows namespace.";

            if (Native.JsProjectWinRTNamespace("JSE") != JavaScriptErrorCode.NoError)
                return "failed to project JSE namespace.";

            // Add references
            await AddScriptReferenceAsync("injection.js");
            var compilationMessage = await AddScriptReferenceAsync("cdc.js");
            if (compilationMessage != "undefined")
            {
                return compilationMessage;
            }

            compilationMessage = await AddScriptHttpReferenceAsync("https://ajax.aspnetcdn.com/ajax/mobileservices/MobileServices.Web-1.1.0.min.js");
            if (compilationMessage != "undefined")
            {
                return compilationMessage;
            }

            compilationMessage = await AddScriptReferenceAsync("cdc-azuremobileservices.js");
            if (compilationMessage != "undefined")
            {
                return compilationMessage;
            }

            // Debug
            if (Native.JsStartDebugging() != JavaScriptErrorCode.NoError)
                return "failed to start debugging.";

            return "NoError";
        }

        public async Task<string> AddScriptReferenceAsync(string name)
        {
            var scriptFile = await CoreTools.GetPackagedFileAsync("refs", name);
            var script = await FileIO.ReadTextAsync(scriptFile);

            return RunScript(script);
        }

        public async Task<string> AddScriptHttpReferenceAsync(string url)
        {
            var script = await CoreTools.DownloadStringAsync(url);

            return RunScript(script);
        }

        public string RunScript(string script)
        {
            IntPtr returnValue;

            try
            {
                JavaScriptValue result;

                if (Native.JsRunScript(script, currentSourceContext++, "", out result) != JavaScriptErrorCode.NoError)
                {
                    // Get error message and clear exception
                    JavaScriptValue exception;
                    if (Native.JsGetAndClearException(out exception) != JavaScriptErrorCode.NoError)
                        return "failed to get and clear exception";
                    
                    JavaScriptPropertyId messageName;
                    if (Native.JsGetPropertyIdFromName("message", 
                        out messageName) != JavaScriptErrorCode.NoError)
                        return "failed to get error message id";

                    JavaScriptValue messageValue;
                    if (Native.JsGetProperty(exception, messageName, out messageValue) 
                        != JavaScriptErrorCode.NoError)
                        return "failed to get error message";

                    IntPtr message;
                    UIntPtr length;
                    if (Native.JsStringToPointer(messageValue, out message, out length) != JavaScriptErrorCode.NoError)
                        return "failed to convert error message";

                    return Marshal.PtrToStringUni(message);
                }

                // Execute promise tasks stored in promiseCallback 
                while (promiseCallback.toIntPtr() != IntPtr.Zero)
                {                
                    JavaScriptValue task = promiseCallback;
                    promiseCallback = new JavaScriptValue(IntPtr.Zero);
                    JavaScriptValue promiseResult;
                    Native.JsCallFunction(task, null, 0, out promiseResult);
                }

                // Convert the return value.
                JavaScriptValue stringResult;
                UIntPtr stringLength;
                if (Native.JsConvertValueToString(result, out stringResult) != JavaScriptErrorCode.NoError)
                    return "failed to convert value to string.";
                if (Native.JsStringToPointer(stringResult, out returnValue, out stringLength) != JavaScriptErrorCode.NoError)
                    return "failed to convert return value.";
            }
            catch (Exception e)
            {
                return "chakrahost: fatal error: internal error: " + e.Message;
            }

            return Marshal.PtrToStringUni(returnValue);
        }
    }
}