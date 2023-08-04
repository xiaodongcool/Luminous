namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     取模分片
    /// </summary>
    public class RemainderSharedAttribute : ShardAttribute
    {
        private readonly short _value;
        private readonly bool _startFrom0;

        public RemainderSharedAttribute(short value, Shared shared, bool startFrom0 = false) : base(shared)
        {
            _value = value;
            _startFrom0 = startFrom0;
        }

        public override string GetSharedSuffix(string value) => (long.Parse(value) % _value + (_startFrom0 ? 0 : 1)).ToString();
    }
}
