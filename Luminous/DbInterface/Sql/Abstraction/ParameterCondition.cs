namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     参数条件
    /// </summary>
    internal class ParameterCondition : Condition
    {
        internal ParameterCondition(string @operator, string propertyName, object propertyValue, string parameterOperator)
        {
            Operator = @operator;
            PropertyName = propertyName;
            PropertyValue = propertyValue;
            ParameterOperator = parameterOperator;
            Type = ConditionType.Parameter;
        }

        /// <summary>
        ///     字段名
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        ///     字段值
        /// </summary>
        public object PropertyValue { get; set; }
        /// <summary>
        ///     操作符
        /// </summary>
        public string ParameterOperator { get; set; }

        public override string ToString()
        {
            return $"[{base.ToString()}, PropertyName:{PropertyName}, PropertyValue:{PropertyValue}, ParameterOperator:{ParameterOperator}]";
        }
    }
}
