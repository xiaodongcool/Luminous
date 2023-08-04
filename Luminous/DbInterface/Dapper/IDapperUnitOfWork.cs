namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper UnitOfWork 接口
    /// </summary>
    public interface IDapperUnitOfWork : IUnitOfWork<RawConnectionContext> { }
}
