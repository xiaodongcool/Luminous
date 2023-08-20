namespace Luminous
{
    /// <summary>
    ///     动态HTTP调用
    /// </summary>
    public interface IDynamicHttpInvocation
    {
        object Get(Type tResponseType, string url, IDictionary<string, string>? header = null);
        object Post(Type tResponseType, string url, string body, IDictionary<string, string>? header = null, string? contentType = null);
    }
}
