namespace Luminous
{
    public class DebugResult<T> : Result<T>, IResult<T>, IDebugResult<T>
    {
        public DebugResult(ResultStatus status, T? payload, string? message, Exception? exception, object? error) : base(status, payload, message)
        {
            if (exception != null)
            {
                Exception = new ConciseExceptionInfo(exception);
            }

            Error = error;
        }

        public object? Error { get; set; }
        public ConciseExceptionInfo? Exception { get; set; }
    }
}
