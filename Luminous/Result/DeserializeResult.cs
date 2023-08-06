using Newtonsoft.Json.Converters;


namespace Luminous
{
    public class DeserializeResult<T> : IResult<T>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ResultStatus Status { get; set; }
        public string? Message { get; set; }
        public T? Payload { get; set; }
        public object? Error { get; set; }
        [JsonIgnore]
        public Exception? Exception { get; set; }
    }
}
