namespace LangM.AspNetCore
{
    /// <summary>
    ///     模型绑定失败信息
    /// </summary>
    public class ModelBindFailEntry
    {
        public string PropertyName { get; set; }
        public object RawValue { get; set; }
        public IList<ErrorItem> Errors { get; set; }
        public class ErrorItem
        {
            public string Message { get; set; }
            public Exception Exception { get; set; }
        }
    }
}
