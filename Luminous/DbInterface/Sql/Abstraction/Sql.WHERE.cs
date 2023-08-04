using System.Linq.Expressions;
using System.Text;

namespace LangM.AspNetCore.DbInterface
{
    public abstract partial class Sql<T>
    {
        protected void AppendWhere(SqlContext ctx, Expression<Func<T, bool>> predicate)
        {
            if (predicate != null)
            {
                ctx.Append(" WHERE");

                //  解析表达式树，返回查询条件
                var conditions = ParsePredicate(predicate.Body);

                var parameterOrder = 0;
                var sharedDb = "";
                var sharedTb = "";

                //  WHERE 条件
                var sql = new StringBuilder();
                //  WHERE 参数
                var parameters = new Dictionary<string, object>();

                BuildWhereSql(conditions, sql, parameters, ref parameterOrder, ref sharedDb, ref sharedTb);

                if (SoftDeletePropertyMetadata != null)
                {
                    ctx.Append($" ({sql}) AND {GetPropertyMetadata(nameof(ISoftDeleteProperties.DeleteFlag)).Column} = 0");
                }
                else
                {
                    ctx.Append(sql.ToString());
                }

                ctx.Parameters = parameters;

                //  优先以表达式树中的分片信息为准
                ctx.SharedDb = sharedDb.Length > 0 ? sharedDb : ctx.SharedDb;
                ctx.SharedTb = sharedTb.Length > 0 ? sharedTb : ctx.SharedTb;
            }
            else
            {
                if (ctx.QueryType == QueryType.Select && SoftDeletePropertyMetadata != null)
                {
                    ctx.Append($" WHERE {SoftDeletePropertyMetadata.Column} = 0");
                }

                if (ctx.QueryType == QueryType.Update || ctx.QueryType == QueryType.Delete)
                {
                    throw new Exception("更新、删除操作必须指定条件");
                }
            }
        }

        private void BuildWhereSql(IList<Condition> conditions, StringBuilder sql, Dictionary<string, object> parameters, ref int parameterOrder, ref string sharedDb, ref string sharedTb)
        {
            foreach (var condition in conditions)
            {
                if (!string.IsNullOrEmpty(condition.Operator))
                {
                    if (sql.Length > 0)
                    {
                        sql.Append(" ");
                    }

                    sql.Append(condition.Operator).Append(" ");
                }

                switch (condition)
                {
                    case ParameterCondition pc:
                        var propertyMetadata = GetPropertyMetadata(pc.PropertyName);
                        var columnName = propertyMetadata.Column;

                        if (pc.PropertyValue != null)
                        {
                            var parameterName = pc.PropertyName + parameterOrder;

                            sql.Append($"{columnName} {pc.ParameterOperator} {ParametersPrefix}{parameterName}");

                            parameters.Add(parameterName, pc.PropertyValue);

                            //  Shared

                            if (_entityReflection.SharedDb?.Property == propertyMetadata.Property)
                            {
                                var temp = _entityReflection.SharedDb.GetSharedSuffix(pc.PropertyValue);

                                if (string.IsNullOrEmpty(sharedDb))
                                {
                                    sharedDb = temp;
                                }
                                else if (string.IsNullOrEmpty(temp) == false)
                                {
                                    if (temp != sharedDb)
                                    {
                                        throw new NotSupportCrossDbSharedException();
                                    }
                                }
                            }

                            if (_entityReflection.SharedTb?.Property == propertyMetadata.Property)
                            {
                                var temp = _entityReflection.SharedTb.GetSharedSuffix(pc.PropertyValue);

                                if (string.IsNullOrEmpty(sharedTb))
                                {
                                    sharedTb = temp;
                                }
                                if (string.IsNullOrEmpty(temp) == false)
                                {
                                    if (temp != sharedTb)
                                    {
                                        throw new NotSupportCrossTbSharedException();
                                    }
                                }
                            }
                        }
                        else
                        {
                            sql.Append($"{columnName} {(pc.ParameterOperator == "=" ? "IS" : "IS NOT")} NULL");
                        }

                        parameterOrder++;
                        break;

                    case CombinationCondition cc:
                        var binarySql = new StringBuilder();
                        var binaryConditions = new Dictionary<string, object>();

                        BuildWhereSql(cc.Conditions, binarySql, binaryConditions, ref parameterOrder, ref sharedDb, ref sharedTb);

                        if (cc.Conditions.Count == 1)
                        {
                            sql.Append(binarySql);
                        }
                        else
                        {
                            sql.Append($"({binarySql})");
                        }

                        parameters.AddRange(binaryConditions);
                        break;
                }
            }
        }

        /// <summary>
        ///     解析表达式树
        /// </summary>
        private IList<Condition> ParsePredicate(Expression expression)
        {
            var condition = ParsePredicate(expression, ExpressionType.Default);
            return condition switch
            {
                ParameterCondition => new List<Condition> { condition },
                CombinationCondition cc => cc.Conditions,
                _ => throw new NotSupportedException(condition.ToString())
            };
        }

        /// <summary>
        ///     解析表达式树
        /// </summary>
        private Condition ParsePredicate(Expression predicate, ExpressionType expressionType)
        {
            var negative = false;

            if (predicate is UnaryExpression unaryExpression)
            {
                if (unaryExpression.NodeType == ExpressionType.Not && unaryExpression.Operand is MethodCallExpression)
                {
                    predicate = unaryExpression.Operand;
                    negative = true;
                }
            }

            if (predicate is MethodCallExpression methodCallExpression)
            {
                var methodName = methodCallExpression.Method.Name;
                var expression = methodCallExpression.Object;
MethodLabel:
                switch (methodName)
                {
                    case "Contains":
                        {
                            if (expression != null && expression.NodeType == ExpressionType.MemberAccess && expression.Type == typeof(string))
                            {
                                methodName = "StringContains";
                                goto MethodLabel;
                            }

                            var propertyName = ExpressionHelper.GetPropertyNamePath(methodCallExpression);

                            if (ExistsProperty(propertyName) == false)
                            {
                                throw new NotSupportPropertyException(propertyName);
                            }

                            var propertyValue = ExpressionHelper.GetValue(methodCallExpression);
                            var parameterOperator = ExpressionHelper.GetSqlOperator(methodName, negative);
                            var @operator = ExpressionHelper.GetSqlOperator(expressionType);

                            return new ParameterCondition(@operator, propertyName, propertyValue, parameterOperator);
                        }
                    case "StringContains":
                    case "CompareString":
                    case "Equals":
                    case "StartsWith":
                    case "EndsWith":
                        {
                            if (expression == null || expression.NodeType != ExpressionType.MemberAccess)
                            {
                                goto default;
                            }

                            var propertyName = ExpressionHelper.GetPropertyNamePath(expression);

                            if (ExistsProperty(propertyName) == false)
                            {
                                throw new NotSupportPropertyException(propertyName);
                            }

                            var propertyValue = ExpressionHelper.GetValuesFromStringMethod(methodCallExpression);
                            var likeValue = ExpressionHelper.GetSqlLikeValue(methodName, propertyValue);
                            var parameterOperator = ExpressionHelper.GetSqlOperator(methodName, negative);
                            var @operator = ExpressionHelper.GetSqlOperator(expressionType);

                            return new ParameterCondition(@operator, propertyName, likeValue, parameterOperator);
                        }
                    case "IsNullOrEmpty":
                        {
                            var arguments = methodCallExpression.Arguments[0];

                            var propertyName = ExpressionHelper.GetPropertyNamePath(arguments);

                            if (ExistsProperty(propertyName) == false)
                            {
                                throw new NotSupportPropertyException(propertyName);
                            }

                            var propertyValue = "";
                            var parameterOperator = ExpressionHelper.GetSqlOperator(methodName, negative);
                            var @operator = ExpressionHelper.GetSqlOperator(expressionType);

                            return new ParameterCondition(@operator, propertyName, propertyValue, parameterOperator);
                        }
                    default:
                        throw new NotSupportMethodException(methodName);
                }
            }

            if (predicate is BinaryExpression binaryExpression)
            {
                if (binaryExpression.NodeType != ExpressionType.AndAlso && binaryExpression.NodeType != ExpressionType.OrElse)
                {
                    var propertyName = ExpressionHelper.GetPropertyNamePath(binaryExpression);

                    if (ExistsProperty(propertyName) == false)
                    {
                        throw new NotSupportPropertyException(propertyName);
                    }

                    var propertyValue = ExpressionHelper.GetValue(binaryExpression.Right);

                    var opr = ExpressionHelper.GetSqlOperator(binaryExpression.NodeType);
                    var link = ExpressionHelper.GetSqlOperator(expressionType);

                    return new ParameterCondition(link, propertyName, propertyValue, opr);
                }

                var leftExpr = ParsePredicate(binaryExpression.Left, ExpressionType.Default);
                var rightExpr = ParsePredicate(binaryExpression.Right, binaryExpression.NodeType);

                switch (leftExpr)
                {
                    case ParameterCondition lQPExpr:
                        if (!string.IsNullOrEmpty(lQPExpr.Operator) && !string.IsNullOrEmpty(rightExpr.Operator)) // AND a AND B
                        {
                            switch (rightExpr)
                            {
                                case CombinationCondition rQBExpr:
                                    if (lQPExpr.Operator == rQBExpr.Conditions.Last().Operator) // AND a AND (c AND d)
                                    {
                                        var nodes = new CombinationCondition(new List<Condition> { leftExpr })
                                        {
                                            Operator = leftExpr.Operator,
                                        };

                                        rQBExpr.Conditions[0].Operator = rQBExpr.Operator;
                                        nodes.Conditions.AddRange(rQBExpr.Conditions);

                                        leftExpr = nodes;
                                        rightExpr = null;
                                        // AND a AND (c AND d) => (AND a AND c AND d)
                                    }

                                    break;
                            }
                        }

                        break;

                    case CombinationCondition lQBExpr:
                        switch (rightExpr)
                        {
                            case ParameterCondition rQPExpr:
                                if (rQPExpr.Operator == lQBExpr.Conditions.Last().Operator) //(a AND b) AND c
                                {
                                    lQBExpr.Conditions.Add(rQPExpr);
                                    rightExpr = null;
                                    //(a AND b) AND c => (a AND b AND c)
                                }

                                break;

                            case CombinationCondition rQBExpr:
                                if (lQBExpr.Conditions.Last().Operator == rQBExpr.Operator) // (a AND b) AND (c AND d)
                                {
                                    if (rQBExpr.Operator == rQBExpr.Conditions.Last().Operator) // AND (c AND d)
                                    {
                                        rQBExpr.Conditions[0].Operator = rQBExpr.Operator;
                                        lQBExpr.Conditions.AddRange(rQBExpr.Conditions);
                                        // (a AND b) AND (c AND d) =>  (a AND b AND c AND d)
                                    }
                                    else
                                    {
                                        lQBExpr.Conditions.Add(rQBExpr);
                                        // (a AND b) AND (c OR d) =>  (a AND b AND (c OR d))
                                    }

                                    rightExpr = null;
                                }

                                break;
                        }

                        break;
                }

                var nLinkingOperator = ExpressionHelper.GetSqlOperator(expressionType);
                if (rightExpr == null)
                {
                    leftExpr.Operator = nLinkingOperator;
                    return leftExpr;
                }

                return new CombinationCondition(new List<Condition> { leftExpr, rightExpr })
                {
                    Type = ConditionType.Combination,
                    Operator = nLinkingOperator,
                };
            }

            return ParsePredicate(ExpressionHelper.GetBinaryExpression(predicate), expressionType);
        }
    }
}
