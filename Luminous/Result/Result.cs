using Newtonsoft.Json.Converters;
using System.Diagnostics;
using System.Reflection;

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
    public static class DynamicResultActivator
    {
        public static object Create(ResultStatus resultStatus, object payload, Type payloadType, string message)
        {
            var constructor = typeof(Result<>).MakeGenericType(payloadType).GetConstructors()[0];

            Debug.Assert(constructor != null);

            var result = constructor.Invoke(new object[] { resultStatus, payload, message });

            return result;
        }
    }
}
