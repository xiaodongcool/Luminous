namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     根据日期分进行分片
    /// </summary>
    public class DaySharedAttribute : TimeSharedAttribute
    {
        public DaySharedAttribute(Shared shared) : base(shared) { }

        public override string GetSharedSuffix(DateTime value) => $"{value.Year}{value.Month:00}{value.Day:00}";
    }
}
