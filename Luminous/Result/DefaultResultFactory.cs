namespace Luminous
{
    public class DefaultResultFactory : IResultFactory
    {
        public IResult<T> Success<T>(T? data = default, string? message = default)
        {
            return new DefaultResult<T>
            {
                Status = ResultStatus.Success,
                Payload = data,
                Message = message ?? "请求成功"
            };
        }

        public IResult<T> Fail<T>(string? message = default, object? error = default, Exception? exception = default)
        {
            return new DefaultResult<T>
            {
                Status = ResultStatus.Fail,
                Message = message ?? "请求失败",
                Error = error,
                Exception = exception
            };
        }

        public IResult<T> ParameterError<T>(string? message)
        {
            return new DefaultResult<T>
            {
                Status = ResultStatus.ParameterError,
                Message = message
            };
        }

        public IResult<T> Create<T>(ResultStatus webApiStatusCode, T? data = default, string? message = default, object? error = default, Exception? exception = default)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = webApiStatusCode switch
                {
                    ResultStatus.Success => "请求成功",
                    ResultStatus.Fail => "请求失败",
                    ResultStatus.ParameterError => "请求参数错误",
                    ResultStatus.UnAuthorized => "未通过授权",
                    ResultStatus.Forbidden => "权限不足",
                    ResultStatus.InternalServerError => "抱歉，服务器刚刚开小差了，请稍后再试",
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

    //public class DebugResultFactory : IResultFactory
    //{

    //}
}
