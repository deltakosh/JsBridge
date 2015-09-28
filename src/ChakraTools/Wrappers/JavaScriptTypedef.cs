namespace ChakraTools
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    ///     A Chakra runtime.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Each Chakra runtime has its own independent execution engine, JIT compiler, and garbage 
    ///     collected heap. As such, each runtime is completely isolated from other runtimes.
    ///     </para>
    ///     <para>
    ///     Runtimes can be used on any thread, but only one thread can call into a runtime at any 
    ///     time.
    ///     </para>
    ///     <para>
    ///     NOTE: A JavaScriptRuntime, unlike other objects in the Chakra hosting API, is not 
    ///     garbage collected since it contains the garbage collected heap itself. A runtime will 
    ///     continue to exist until Dispose is called.
    ///     </para>
    /// </remarks>
    internal struct JavaScriptRuntime : IDisposable
    {
        /// <summary>
        /// The handle.
        /// </summary>
        private IntPtr handle;

        /// <summary>
        ///     Gets a value indicating whether the runtime is valid.
        /// </summary>
        public bool IsValid
        {
            get { return handle != IntPtr.Zero; }
        }

        /// <summary>
        ///     Disposes a runtime.
        /// </summary>
        /// <remarks>
        ///     Once a runtime has been disposed, all resources owned by it are invalid and cannot be used.
        ///     If the runtime is active (i.e. it is set to be current on a particular thread), it cannot 
        ///     be disposed.
        /// </remarks>
        public void Dispose()
        {
            if (IsValid)
            {
                Native.JsDisposeRuntime(this);
            }

            handle = IntPtr.Zero;
        }

    }

    /// <summary>
    ///     A script context.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Each script context contains its own global object, distinct from the global object in 
    ///     other script contexts.
    ///     </para>
    ///     <para>
    ///     Many Chakra hosting APIs require an "active" script context, which can be set using 
    ///     Current. Chakra hosting APIs that require a current context to be set will note 
    ///     that explicitly in their documentation.
    ///     </para>
    /// </remarks>
    internal struct JavaScriptContext
    {
        /// <summary>
        ///     The reference.
        /// </summary>
        private readonly IntPtr reference;
    }

    /// <summary>
    ///     A cookie that identifies a script for debugging purposes.
    /// </summary>
    internal struct JavaScriptSourceContext : IEquatable<JavaScriptSourceContext>
    {
        /// <summary>
        /// The context.
        /// </summary>
        private readonly IntPtr context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptSourceContext"/> struct.
        /// </summary>
        /// <param name="context">The context.</param>
        private JavaScriptSourceContext(IntPtr context)
        {
            this.context = context;
        }

        /// <summary>
        ///     Increments the value of the source context.
        /// </summary>
        /// <param name="context">The source context to increment.</param>
        /// <returns>A new source context that reflects the incrementing of the context.</returns>
        public static JavaScriptSourceContext operator ++(JavaScriptSourceContext context)
        {
            return FromIntPtr(context.context + 1);
        }

        /// <summary>
        ///     Creates a new source context. 
        /// </summary>
        /// <param name="cookie">
        ///     The cookie for the source context.
        /// </param>
        /// <returns>The new source context.</returns>
        public static JavaScriptSourceContext FromIntPtr(IntPtr cookie)
        {
            return new JavaScriptSourceContext(cookie);
        }

        /// <summary>
        ///     Checks for equality between source contexts.
        /// </summary>
        /// <param name="other">The other source context to compare.</param>
        /// <returns>Whether the two source contexts are the same.</returns>
        public bool Equals(JavaScriptSourceContext other)
        {
            return context == other.context;
        }

        /// <summary>
        ///     Checks for equality between source contexts.
        /// </summary>
        /// <param name="obj">The other source context to compare.</param>
        /// <returns>Whether the two source contexts are the same.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is JavaScriptSourceContext && Equals((JavaScriptSourceContext)obj);
        }

        /// <summary>
        ///     The hash code.
        /// </summary>
        /// <returns>The hash code of the source context.</returns>
        public override int GetHashCode()
        {
            return context.ToInt32();
        }
    }

    /// <summary>
    ///     A background work item callback.
    /// </summary>
    /// <remarks>
    ///     This is passed to the host's thread service (if provided) to allow the host to 
    ///     invoke the work item callback on the background thread of its choice.
    /// </remarks>
    /// <param name="callbackData">Data argument passed to the thread service.</param>
    internal delegate void JavaScriptBackgroundWorkItemCallback(IntPtr callbackData);

    /// <summary>
    ///     A callback called before collection.
    /// </summary>
    /// <param name="callbackState">The state passed to SetBeforeCollectCallback.</param>
    internal delegate void JavaScriptBeforeCollectCallback(IntPtr callbackState);

    /// <summary>
    ///     User implemented callback routine for memory allocation events
    /// </summary>
    /// <param name="callbackState">The state passed to SetRuntimeMemoryAllocationCallback.</param>
    /// <param name="allocationEvent">The type of type allocation event.</param>
    /// <param name="allocationSize">The size of the allocation.</param>
    /// <returns>
    ///     For the Allocate event, returning true allows the runtime to continue with 
    ///     allocation. Returning false indicates the allocation request is rejected. The return value
    ///     is ignored for other allocation events.
    /// </returns>
    internal delegate bool JavaScriptMemoryAllocationCallback(IntPtr callbackState, JavaScriptMemoryEventType allocationEvent, UIntPtr allocationSize);

    /// <summary>
    ///     A finalization callback.
    /// </summary>
    /// <param name="data">
    ///     The external data that was passed in when creating the object being finalized.
    /// </param>
    internal delegate void JavaScriptObjectFinalizeCallback(IntPtr data);

    /// <summary>
    ///     A thread service callback.
    /// </summary>
    /// <remarks>
    ///     The host can specify a background thread service when creating a runtime. If 
    ///     specified, then background work items will be passed to the host using this callback. The
    ///     host is expected to either begin executing the background work item immediately and return
    ///     true or return false and the runtime will handle the work item in-thread.
    /// </remarks>
    /// <param name="callbackFunction">The callback for the background work item.</param>
    /// <param name="callbackData">The data argument to be passed to the callback.</param>
    /// <returns>Whether the thread service will execute the callback.</returns>
    internal delegate bool JavaScriptThreadServiceCallback(JavaScriptBackgroundWorkItemCallback callbackFunction, IntPtr callbackData);

    /// <summary>
    ///     A function callback.
    /// </summary>
    /// <param name="callee">
    ///     A <c>Function</c> object that represents the function being invoked.
    /// </param>
    /// <param name="isConstructCall">Indicates whether this is a regular call or a 'new' call.</param>
    /// <param name="arguments">The arguments to the call.</param>
    /// <param name="argumentCount">The number of arguments.</param>
    /// <param name="callbackData">Callback data, if any.</param>
    /// <returns>The result of the call, if any.</returns>
    internal delegate JavaScriptValue JavaScriptNativeFunction(JavaScriptValue callee, bool isConstructCall, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 3)] JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData);

    /// <summary>
    ///     A callback called before collecting an object.
    /// </summary>
    /// <remarks>
    ///     Use <c>JsSetObjectBeforeCollectCallback</c> to register this callback.
    /// </remarks>
    /// <param name="ref">The object to be collected.</param>
    /// <param name="callbackState">The state passed to <c>JsSetObjectBeforeCollectCallback</c>.</param>
    internal delegate void JavaScriptObjectBeforeCollectCallback(JavaScriptValue reference, IntPtr callbackState);

    /// <summary>
    ///     A promise continuation callback.
    /// </summary>
    /// <remarks>
    ///     The host can specify a promise continuation callback in <c>JsSetPromiseContinuationCallback</c>. If
    ///     a script creates a task to be run later, then the promise continuation callback will be called with
    ///     the task and the task should be put in a FIFO queue, to be run when the current script is
    ///     done executing.
    /// </remarks>
    /// <param name="task">The task, represented as a JavaScript function.</param>
    /// <param name="callbackState">The data argument to be passed to the callback.</param>
    internal delegate void JavaScriptPromiseContinuationCallback(JavaScriptValue task, IntPtr callbackState);
}
