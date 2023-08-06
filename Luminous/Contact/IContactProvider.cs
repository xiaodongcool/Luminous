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
        IResult<T> Create<T>(WebApiStatusCode webApiStatusCode, T data = default, string message = default, object error = default, Exception exception = default);
    }

    public class DefaultResultFactory : IResultFactory
    {
        public IResult<T> Success<T>(T data = default, string message = default)
        {
            return new DefaultResult<T>
            {
                Status = WebApiStatusCode.Success,
                Payload = data,
                Message = message ?? "请求成功"
            };
        }

        public IResult<T> Fail<T>(string message = default, object error = default, Exception exception = default)
        {
            return new DefaultResult<T>
            {
                Status = WebApiStatusCode.Fail,
                Message = message ?? "请求失败",
                Error = error,
                Exception = exception
            };
        }

        public IResult<T> ParameterError<T>(string message)
        {
            return new DefaultResult<T>
            {
                Status = WebApiStatusCode.ParameterError,
                Message = message
            };
        }

        public IResult<T> Create<T>(WebApiStatusCode webApiStatusCode, T data = default, string message = default, object error = default, Exception exception = default)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = webApiStatusCode switch
                {
                    WebApiStatusCode.Success => "请求成功",
                    WebApiStatusCode.Fail => "请求失败",
                    WebApiStatusCode.ParameterError => "请求参数错误",
                    WebApiStatusCode.UnAuthorized => "未通过授权",
                    WebApiStatusCode.Forbidden => "权限不足",
                    WebApiStatusCode.InternalServerError => "抱歉，服务器刚刚开小差了，请稍后再试",
                    _ => "未知错误"
                };
            }

            return new DefaultResult<T>
            {
                Status = webApiStatusCode,
                Message = message,
                Error = error,
                Exception = exception,
                Payload = data
            };
        }
    }
}
