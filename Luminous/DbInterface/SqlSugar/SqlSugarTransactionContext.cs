using SqlSugar;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     SqlSugar 事务上下文
    /// </summary>
    public class SqlSugarTransactionContext : ITransactionContext
    {
        private readonly SqlSugarClient _connection;

        public SqlSugarTransactionContext(SqlSugarClient connection)
        {
            _connection = connection;
        }

        public Task Commit()
        {
            _connection.CommitTran();
            return Task.CompletedTask;
        }

        public Task RollBack()
        {
            _connection.RollbackTran();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            try
            {
                _connection.CommitTran();
            }
            catch
            {
                //  TODO:log
                try
                {
                    _connection.RollbackTran();
                }
                catch { }
            }
        }
    }
}
