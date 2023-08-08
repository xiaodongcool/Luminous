using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Luminous
{
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
}
