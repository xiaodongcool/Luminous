namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     工作单元
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        Task<ITransactionContext> StartTransaction(string dbName = null, string split = null);
    }

    /// <summary>
    ///     工作单元
    /// </summary>
    public interface IUnitOfWork<TConnection> : IUnitOfWork
    {
        TConnection GetConnection(string dbName = null, string split = null);
    }
}
