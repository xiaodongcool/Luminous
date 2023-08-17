namespace Luminous
{
    /// <summary>
    ///     模型绑定失败信息
    /// </summary>
    public class ModelBindFail
    {
        public string Name { get; set; } = null!;
        public object? Value { get; set; }
        public ModelBindFailDetail[] Errors { get; set; } = null!;
    }
    public class ModelBindFailDetail
    {
        public string Message { get; set; } = null!;
        public Exception? Exception { get; set; }
    }
}
