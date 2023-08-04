namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper 仓储接口
    /// </summary>
    public interface IDapperRepository<T> : IRepository<T> where T : class, IEntity, new() { }
}
