using System;

namespace Luminous.Exception
{
    /// <summary>
    ///     抛出一个已知的异常，中断客户端的本次请求，并返回一个提示信息
    /// </summary>
    public class FailException : Exception
    {
        /// <summary>
        ///     状态码
        /// </summary>
        public ResultStatus StatusCode { get; set; }

        /// <summary>
        ///     返回给客户端的提示消息,不要返回敏感信息
        /// </summary>
        public string? ResultMessage { get; set; }

        /// <summary>
        ///     是否在全局异常中记录日志
        /// </summary>
        public bool LogOnGlobalException { get; set; }

        /// <summary>
        ///     内部异常
        /// </summary>
        public Exception? Exception { get; set; }

        public FailException(string? resultMessage = null) : this(resultMessage, null, false)
        {
        }

        public FailException(string? resultMessage, Exception? exception) : this(resultMessage, exception, false)
        {

        }

        protected FailException(string? resultMessage, Exception? exception, bool logOnGlobalException) : this(ResultStatus.Fail, resultMessage, exception, logOnGlobalException)
        {
        }

        protected FailException(ResultStatus resultStatus, string? resultMessage, Exception? exception, bool logOnGlobalException) : base(resultMessage, exception)
        {
            ResultMessage = resultMessage;
            StatusCode = resultStatus;
            Exception = exception;
        }
    }

    public class ParameterErrorException : FailException
    {
        public ParameterErrorException(string resultMessage) : base(ResultStatus.ParameterError, resultMessage, null, false) { }
        public ParameterErrorException(string resultMessage, bool logOnGlobalException) : base(ResultStatus.ParameterError, resultMessage, null, logOnGlobalException) { }
    }

    public class NotFoundException : Exception
    {

    }
}
