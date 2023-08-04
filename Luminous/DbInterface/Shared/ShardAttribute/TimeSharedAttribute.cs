namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     根据时间进行分片
    /// </summary>
    public abstract class TimeSharedAttribute : ShardAttribute
    {
        protected TimeSharedAttribute(Shared shared) : base(shared) { }

        public override string GetSharedSuffix(string value)
        {
            return GetSharedSuffix(DateTime.Parse(value));
        }

        public abstract string GetSharedSuffix(DateTime value);
    }
}
