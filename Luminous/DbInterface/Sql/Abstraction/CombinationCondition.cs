namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     组合条件
    /// </summary>
    internal class CombinationCondition : Condition
    {
        public CombinationCondition(IList<Condition> conditions)
        {
            Conditions = conditions;
            Type = ConditionType.Combination;
        }

        public IList<Condition> Conditions { get; }

        public override string ToString()
        {
            return $"[{base.ToString()} ({string.Join(",", Conditions)})]";
        }
    }
}
