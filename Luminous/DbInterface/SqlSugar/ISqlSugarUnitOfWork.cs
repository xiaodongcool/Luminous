using SqlSugar;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     SqlSugar UnitOfWork 接口
    /// </summary>
    public interface ISqlSugarUnitOfWork : IUnitOfWork<SqlSugarClient> { }
}
