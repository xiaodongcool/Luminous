using System.Data;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     原始的数据库连接上下文
    /// </summary>
    public class RawConnectionContext
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
    }
}
