using Newtonsoft.Json.Converters;


namespace Luminous
{
    //public interface IResult : IResult<object> { }

    public class DefaultResult<T> : IResult<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultStatus Status { get; set; }
        public string? Message { get; set; }
        public T? Payload { get; set; }
        public object? Error { get; set; }
        public Exception? Exception { get; set; }
    }
}
