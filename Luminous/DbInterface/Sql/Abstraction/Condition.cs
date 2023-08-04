namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     查询条件
    /// </summary>
    internal abstract class Condition
    {
        public ConditionType Type { get; set; }

        public string Operator { get; set; }

        public override string ToString()
        {
            return $"[Type:{Type}, Operator:{Operator}]";
        }
    }
}
