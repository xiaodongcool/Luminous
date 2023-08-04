namespace Luminous
{
    /// <summary>
    ///     抛出一个已知的异常，中断客户端的本次请求，并返回一个提示信息
    /// </summary>
    public class ForegoneException : Exception
    {
        /// <summary>
        ///     返回给客户端的提示消息,不要返回敏感信息
        /// </summary>
        public string ApiMessage { get; set; }

        /// <summary>
        ///     状态码
        /// </summary>
        public WebApiStatusCode StatusCode { get; set; }

        /// <summary>
        ///     是否被全局异常捕捉
        /// </summary>
        public bool CatchGlobalException { get; set; }

        /// <summary>
        ///     内部异常
        /// </summary>
        public Exception Exception { get; set; }

        public ForegoneException(string message, Exception exception = null, bool catchGlobalException = false) : base(message, exception)
        {
            StatusCode = WebApiStatusCode.Fail;
            ApiMessage = message;
            CatchGlobalException = catchGlobalException;
            Exception = exception;
        }

        public ForegoneException(WebApiStatusCode code, string message, Exception exception = null, bool catchGlobalException = false) : base(message, exception)
        {
            StatusCode = code;
            ApiMessage = message;
            CatchGlobalException = catchGlobalException;
            Exception = exception;
        }
    }
}
