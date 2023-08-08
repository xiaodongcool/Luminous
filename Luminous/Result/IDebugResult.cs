namespace Luminous
{
    public interface IDebugResult<T> : IResult<T>
    {
        /// <summary>
        ///     错误信息(生产环境关闭)
        /// </summary>
        object? Error { get; set; }

        /// <summary>
        ///     异常信息(生产环境关闭)
        /// </summary>
        ConciseExceptionInfo? Exception { get; set; }
    }
}
