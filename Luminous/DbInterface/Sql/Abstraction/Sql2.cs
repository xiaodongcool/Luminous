using System.Linq.Expressions;
using System.Text;

namespace LangM.AspNetCore.DbInterface
{
    public abstract partial class Sql<T>
    {
        protected void AppendWhere2(SqlContext ctx, Expression<Func<T, bool>> predicate)
        {
        }
    }
}
