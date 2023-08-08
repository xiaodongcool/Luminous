namespace Luminous
{
    public class ConciseExceptionInfo
    {
        public ConciseExceptionInfo(Exception exception)
        {
            Type = exception.GetType().FullName;
            Message = exception.Message;
            Source = exception.Source;
            Data = exception.Data;

            if (exception.InnerException != null)
            {
                InnerException = new ConciseExceptionInfo(exception.InnerException);
            }

            StackTrace = ExceptionUtil.GetOrderedStackTrace(exception);
        }

        public string? Type { get; set; }
        public string? Message { get; set; }
        public string? Source { get; set; }
        public IDictionary Data { get; set; }
        public ConciseExceptionInfo? InnerException { get; set; }
        public StackFrameInfo[] StackTrace { get; set; }
    }
}
