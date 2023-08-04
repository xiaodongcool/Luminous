using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace LangM.AspNetCore.DbInterface
{
    internal static class ExpressionHelper
    {
        public static object GetValue(Expression? member)
        {
            return GetValue(member, out _);
        }

        private static object GetValue(Expression? member, out string? parameterName)
        {
            parameterName = null;
            if (member == null)
                return null;

            switch (member)
            {
                case MemberExpression memberExpression:
                    var instanceValue = GetValue(memberExpression.Expression, out parameterName);
                    try
                    {
                        switch (memberExpression.Member)
                        {
                            case FieldInfo fieldInfo:
                                parameterName = (parameterName != null ? parameterName + "_" : "") + fieldInfo.Name;
                                return fieldInfo.GetValue(instanceValue);

                            case PropertyInfo propertyInfo:
                                parameterName = (parameterName != null ? parameterName + "_" : "") + propertyInfo.Name;
                                return propertyInfo.GetValue(instanceValue);
                        }
                    }
                    catch
                    {
                        // Try again when we compile the delegate
                    }

                    break;

                case ConstantExpression constantExpression:
                    return constantExpression.Value;

                case MethodCallExpression methodCallExpression:
                    parameterName = methodCallExpression.Method.Name;
                    break;

                case UnaryExpression unaryExpression
                    when (unaryExpression.NodeType == ExpressionType.Convert
                          || unaryExpression.NodeType == ExpressionType.ConvertChecked)
                         && (unaryExpression.Type.UnwrapNullableType() == unaryExpression.Operand.Type):
                    return GetValue(unaryExpression.Operand, out parameterName);
            }

            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }

        public static string GetSqlOperator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal:
                case ExpressionType.Not:
                case ExpressionType.MemberAccess:
                    return "=";

                case ExpressionType.NotEqual:
                    return "!=";

                case ExpressionType.LessThan:
                    return "<";

                case ExpressionType.LessThanOrEqual:
                    return "<=";

                case ExpressionType.GreaterThan:
                    return ">";

                case ExpressionType.GreaterThanOrEqual:
                    return ">=";

                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    return "AND";

                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    return "OR";

                case ExpressionType.Default:
                    return string.Empty;

                default:
                    throw new NotSupportOperateException(type);
            }
        }

        public static string GetSqlLikeValue(string methodName, object value)
        {
            value ??= string.Empty;

            switch (methodName)
            {
                case "CompareString":
                case "Equals":
                    return value.ToString() ?? string.Empty;

                case "StartsWith":
                    return string.Format("{0}%", value);

                case "EndsWith":
                    return string.Format("%{0}", value);

                case "StringContains":
                    return string.Format("%{0}%", value);

                default:
                    throw new NotImplementedException();
            }
        }

        public static string GetSqlOperator(string methodName, bool negative)
        {
            switch (methodName)
            {
                case "StartsWith":
                case "EndsWith":
                case "StringContains":
                    return negative ? "NOT LIKE" : "LIKE";

                case "Contains":
                    return negative ? "NOT IN" : "IN";

                case "Equals":
                case "IsNullOrEmpty":
                case "CompareString":
                    return negative ? "!=" : "=";
                case "Any":
                case "All":
                    return methodName.ToUpperInvariant();

                default:
                    throw new NotSupportMethodException(methodName);
            }
        }

        public static BinaryExpression GetBinaryExpression(Expression expression)
        {
            var binaryExpression = expression as BinaryExpression;
            var body = binaryExpression ?? Expression.MakeBinary(ExpressionType.Equal, expression,
                expression.NodeType == ExpressionType.Not ? Expression.Constant(false) : Expression.Constant(true));
            return body;
        }

        public static Func<PropertyInfo, bool> GetPrimitivePropertiesPredicate()
        {
            return p => p.CanWrite && (p.PropertyType.IsValueType || p.GetCustomAttributes<ColumnAttribute>().Any() || p.PropertyType == typeof(string) ||
                                       p.PropertyType == typeof(byte[]));
        }

        public static object GetValuesFromStringMethod(MethodCallExpression callExpr)
        {
            var expr = callExpr.Method.IsStatic ? callExpr.Arguments[1] : callExpr.Arguments[0];

            return GetValue(expr);
        }

        public static object GetValue(MethodCallExpression callExpr)
        {
            try
            {
                MemberExpression expr;

                if (callExpr.Method.IsStatic)
                {
                    var argument = callExpr.Arguments.First();

                    expr = argument as MemberExpression;

                    if (argument is MemberExpression)
                    {
                        return GetValue(expr);
                    }
                    else if (argument is NewArrayExpression array)
                    {
                        return array.Expressions.Select(_ => (ConstantExpression)_).Select(_ => _.Value).ToArray();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    expr = callExpr.Object as MemberExpression;

                    return GetValue(expr);
                }

                //System.Linq.Expressions.FieldExpression

                //NewArrayExpression

                //if (expr == null)
                //{
                //    return null;
                //}
                //else
                //{
                //    return GetValue(expr);
                //}
            }
            catch
            {
                throw new NotSupportedException(callExpr.Method.Name + " isn't supported");
            }
        }

        public static MemberExpression GetMemberExpression(Expression expression)
        {
            //  MemberExpression 表示访问字段或属性
            switch (expression)
            {
                case MethodCallExpression expr:
                    //  表示对静态方法或实例方法的调用。

                    if (expr.Method.IsStatic)
                        return (MemberExpression)expr.Arguments.Last(x => x.NodeType == ExpressionType.MemberAccess);
                    else
                        return (MemberExpression)expr.Arguments[0];

                case MemberExpression memberExpression:
                    //  表示访问字段或属性。

                    return memberExpression;

                case UnaryExpression unaryExpression:
                    //  表示具有一元运算符的表达式

                    return (MemberExpression)unaryExpression.Operand;

                case BinaryExpression binaryExpression:
                    //  表示具有二进制运算符的表达式
                    var binaryExpr = binaryExpression;

                    //  内部表达式
                    //  System.Linq.Expressions.PropertyExpression a;
                    //  System.Linq.Expressions.MethodBinaryExpression a;

                    if (binaryExpr.Left is UnaryExpression left)
                        return (MemberExpression)left.Operand;

                    //should we take care if right operation is memberaccess, not left?
                    return (MemberExpression)binaryExpr.Left;

                case LambdaExpression lambdaExpression:
                    //  介绍 lambda 表达式。 它捕获一个类似于 .NET 方法主体的代码块。

                    switch (lambdaExpression.Body)
                    {
                        case MemberExpression body:
                            return body;

                        case UnaryExpression expressionBody:
                            return (MemberExpression)expressionBody.Operand;
                    }

                    break;
            }

            return null;
        }

        public static string GetPropertyNamePath(Expression expression)
        {
            var path = new StringBuilder();

            var memberExpression = GetMemberExpression(expression);
            var count = 0;
            while (memberExpression != null)
            {
                count++;

                if (path.Length > 0)
                {
                    path.Insert(0, memberExpression.Member.Name + ".");
                }
                else
                {
                    path.Insert(0, memberExpression.Member.Name);
                }

                memberExpression = GetMemberExpression(memberExpression.Expression);
            }

            if (count > 2)
            {
                throw new NotSupportNestException();
            }

            return path.ToString();
        }
    }
}
