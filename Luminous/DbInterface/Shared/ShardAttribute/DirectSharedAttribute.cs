namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     最直接的分片方式，根据属性值进行分片，属性值是123456就落进123456库、表
    /// </summary>
    public class DirectSharedAttribute : ShardAttribute
    {
        public DirectSharedAttribute(Shared shared) : base(shared) { }

        public override string GetSharedSuffix(string value) => value;
    }
}
