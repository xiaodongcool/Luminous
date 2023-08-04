using SqlSugar;
using System.Collections.Concurrent;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     SqlSugar UnitOfWork
    /// </summary>
    public class SqlSugarUnitOfWork : ISqlSugarUnitOfWork
    {
        private readonly ConcurrentDictionary<string, SqlSugarClient> _connections = new();
        private readonly IConnectionStringProvider _connectionStringProvider;

        public SqlSugarUnitOfWork(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        private SqlSugarClient CreateConnection(string dbName, string split)
        {
            if (Empty(dbName))
            {
                dbName = _connectionStringProvider.DefaultDb;
            }

            var key = dbName + split;

            if (_connections.TryGetValue(key, out var connection))
            {
                return connection;
            }

            var connectionString = _connectionStringProvider.Get(dbName, split);
            connection = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                ConfigId = $"{dbName}{split}"
            });
#if DEBUG
            connection.Aop.OnLogExecuting = (sql, pars) =>
            {
                System.Diagnostics.Debug.WriteLine("\r\n");
                System.Diagnostics.Debug.WriteLine("================SqlSugar DEBUG Start============");
                System.Diagnostics.Debug.WriteLine("\r\n");
                System.Diagnostics.Debug.WriteLine(sql + "\r\n" + connection.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                System.Diagnostics.Debug.WriteLine("\r\n");
                System.Diagnostics.Debug.WriteLine("\r\n");
                System.Diagnostics.Debug.WriteLine("================SqlSugar DEBUG End============");
            };
#endif
            _connections.TryAdd(key, connection);

            return connection;
        }

        public SqlSugarClient GetConnection(string dbName = null, string split = null)
        {
            return CreateConnection(dbName, split);
        }

        public Task<ITransactionContext> StartTransaction(string dbName = null, string split = null)
        {
            var connection = CreateConnection(dbName, split);

            connection.BeginTran();

            return Task.FromResult(new SqlSugarTransactionContext(connection) as ITransactionContext);
        }

        public void Dispose()
        {
            foreach (var (name, connection) in _connections)
            {
                connection?.Close();
                connection?.Dispose();
            }
        }
    }
}
