using Newtonsoft.Json.Converters;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
            if (exception != null)
            {
                Exception = new ConciseExceptionInfo(exception);
            }

            Error = error;
        }

        public object? Error { get; set; }
        public ConciseExceptionInfo? Exception { get; set; }
    }

    public class ExceptionUtil
    {
        public static StackFrameInfo[] GetOrderedStackTrace(Exception exception)
        {
            if (exception == null)
            {
                return Array.Empty<StackFrameInfo>();
            }

            var stackTrace = new StackTrace(exception, true);

            var frameInfos = new List<StackFrameInfo>();

            foreach (var frame in stackTrace.GetFrames())
            {
                var filename = frame.GetFileName();

                if (filename?.EndsWith(".cs") == true)
                {
                    var method = GetRealMethod(frame);

                    var frameInfo = new StackFrameInfo
                    {
                        FilePath = filename,
                        LineNumber = frame.GetFileLineNumber(),
                        MethodName = method.MethodName,
                        TypeName = method.TypeName
                    };


                    frameInfos.Add(frameInfo);
                }
            }

            return frameInfos.ToArray();
        }

        public static (string TypeName, string MethodName) GetRealMethod(StackFrame stackFrame)
        {
            var methodBase = stackFrame.GetMethod();

            var typeName = GetRealTypeName(methodBase.DeclaringType);

            var methodName = methodBase.Name;

            if (methodBase.Name == "MoveNext")
            {
                methodName = ExtractContentFromAngleBrackets(methodBase.DeclaringType.Name);
            }

            return (typeName, methodName);
        }

        public static string GetRealTypeName(Type type)
        {
            if (type == null)
            {
                return "";
            }

            if (type.FullName.Contains('<'))
            {
                return GetRealTypeName(type.DeclaringType);
            }

            return type.FullName;
        }

        public static string ExtractContentFromAngleBrackets(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var pattern = @"<(.*?)>";
            var match = Regex.Match(input, pattern);

            if (match.Success)
            {
                var content = match.Groups[1].Value;
                return content;
            }
            else
            {
                return string.Empty;
            }
        }
    }

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

    public class StackFrameInfo
    {
        public string FilePath { get; set; } = null!;
        public int LineNumber { get; set; }
        public string MethodName { get; set; } = null!;
        public string TypeName { get; set; } = null!;
    }
}
