namespace LangM.AspNetCore
{
    /// <summary>
    ///     分页赋值器
    /// </summary>
    public interface IPagingAssignment : IAssignment
    {
        public int PageIndex { get; }
        public int PageSize { get; }
    }
}
