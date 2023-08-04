namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     哈希取模分片
    /// </summary>
    public class HashRemainderSharedAttribute : RemainderSharedAttribute
    {
        private readonly short _value;
        private readonly bool _startFrom0;

        public HashRemainderSharedAttribute(short value, Shared shared, bool startFrom0 = false) : base(value, shared, startFrom0)
        {
            _value = value;
            _startFrom0 = startFrom0;
        }

        public override string GetSharedSuffix(string value) => (Math.Abs((value ?? "").GetHashCodePermanent()) % _value + (_startFrom0 ? 0 : 1)).ToString();
    }
}
