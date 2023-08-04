namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     根据年分进行分片
    /// </summary>
    public class YearSharedAttribute : TimeSharedAttribute
    {
        public YearSharedAttribute(Shared shared) : base(shared) { }

        public override string GetSharedSuffix(DateTime value) => value.Year.ToString();
    }
}
