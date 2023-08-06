namespace Luminous
{
    public interface IResultFactory
    {
        /// <summary>
        ///     返回一个成功的响应报文
        /// </summary>
        /// <param name="data">响应数据</param>
        /// <param name="message">提示消息</param>
        IResult<T> Success<T>(T data = default, string message = default);

        /// <summary>
        ///     返回一个失败的响应报文
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="error">错误信息</param>
        /// <param name="exception">异常</param>
        IResult<T> Fail<T>(string message = default, object error = default, Exception exception = default);

        /// <summary>
        ///     返回一个失败的响应报文(入参错误)
        /// </summary>
        /// <param name="message">提示消息</param>
        IResult<T> ParameterError<T>(string message);

        /// <summary>
        ///     创建一个响应报文
        /// </summary>
        /// <param name="webApiStatusCode">响应状态码</param>
        /// <param name="data">响应数据</param>
        /// <param name="message">提示消息</param>
        /// <param name="error">错误信息</param>
        /// <param name="exception">异常</param>
        IResult<T> Create<T>(ResultStatus webApiStatusCode, T data = default, string message = default, object error = default, Exception exception = default);
    }
}
