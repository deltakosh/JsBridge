namespace ChakraTools
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    ///     A script exception.
    /// </summary>
    internal class JavaScriptScriptException : JavaScriptException
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptScriptException"/> class. 
        /// </summary>
        /// <param name="code">The error code returned.</param>
        /// <param name="error">The JavaScript error object.</param>
        public JavaScriptScriptException(JavaScriptErrorCode code, JavaScriptValue error) :
            this(code, error, "JavaScript Exception")
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="JavaScriptScriptException"/> class. 
        /// </summary>
        /// <param name="code">The error code returned.</param>
        /// <param name="error">The JavaScript error object.</param>
        /// <param name="message">The error message.</param>
        public JavaScriptScriptException(JavaScriptErrorCode code, JavaScriptValue error, string message) :
            base(code, message)
        {
            this.Error = error;
        }

        /// <summary>
        ///     Gets a JavaScript object representing the script error.
        /// </summary>
        public JavaScriptValue Error { get; }
    }
}