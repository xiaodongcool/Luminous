using Newtonsoft.Json.Converters;


namespace Luminous
{
    public class Result<T> : IResult<T>
    {
        public Result(ResultStatus status, T? payload, string? message)
        {
            Status = status;
            Message = message;
            Payload = payload;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public ResultStatus Status { get; set; }
        public T? Payload { get; set; }
        public string? Message { get; set; }
    }

    public class DebugResult<T> : Result<T>, IResult<T>, IDebugResult<T>
    {
        public DebugResult(ResultStatus status, T? payload, string? message, Exception? exception, object? error) : base(status, payload, message)
        {
            Exception = exception;
            Error = error;
        }

        public string[]? ErrorCodeLocation { get; set; }
        public object? Error { get; set; }
        public Exception? Exception { get; set; }
    }
}
