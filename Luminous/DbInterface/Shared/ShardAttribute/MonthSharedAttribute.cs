namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     根据月分进行分片
    /// </summary>
    public class MonthSharedAttribute : TimeSharedAttribute
    {
        public MonthSharedAttribute(Shared shared) : base(shared) { }

        public override string GetSharedSuffix(DateTime value) => $"{value.Year}{value.Month:00}";
    }
}
