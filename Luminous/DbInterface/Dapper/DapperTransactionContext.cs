using System.Data;
using Microsoft.Extensions.Logging;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper 事务上下文
    /// </summary>
    public class DapperTransactionContext : ITransactionContext
    {
        private readonly IDbTransaction _dbTransaction;

        public DapperTransactionContext(IDbTransaction dbTransaction)
        {
            _dbTransaction = dbTransaction;
        }

        public Task Commit()
        {
            if (_dbTransaction.Connection is { State: ConnectionState.Open })
            {
                _dbTransaction.Commit();
            }

            return Task.CompletedTask;
        }
        public Task RollBack()
        {
            _dbTransaction.Rollback();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            try
            {
                if (_dbTransaction.Connection is { State: ConnectionState.Open })
                {
                    _dbTransaction.Commit();
                }
            }
            catch (Exception e)
            {
                //  TODO:log
                try
                {
                    _dbTransaction.Rollback();
                }
                catch { }
            }
        }

    }
}
