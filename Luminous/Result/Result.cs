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
}
