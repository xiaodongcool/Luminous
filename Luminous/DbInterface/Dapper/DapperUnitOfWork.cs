using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using System.Data;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     Dapper UnitOfWork
    /// </summary>
    public class DapperUnitOfWork : IDapperUnitOfWork
    {
        private readonly ConcurrentDictionary<string, RawConnectionContext> _connections = new();
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IShardEnsured _shardEnsured;

        public DapperUnitOfWork(IConnectionStringProvider connectionStringProvider, IShardEnsured shardEnsured)
        {
            _connectionStringProvider = connectionStringProvider;
            _shardEnsured = shardEnsured;
        }

        public RawConnectionContext GetConnection(string dbName, string split)
        {
            return CreateConnection(dbName, split);
        }

        public async Task<ITransactionContext> StartTransaction(string dbName, string split)
        {
            var entry = CreateConnection(dbName, split);

            if (entry.Connection.State != ConnectionState.Open)
            {
                entry.Connection.Open();
            }

            entry.Transaction = entry.Connection.BeginTransaction(IsolationLevel.ReadUncommitted);

            return new DapperTransactionContext(entry.Transaction) as ITransactionContext;
        }

        private RawConnectionContext CreateConnection(string dbName, string split)
        {
            if (Empty(dbName))
            {
                dbName = _connectionStringProvider.DefaultDb;
            }

            var key = dbName + split;

            if (_connections.TryGetValue(key, out var entry))
            {
                return entry;
            }

            //  TODO：创建连接是很平常的，要改成异步方法
            //  TODO:这边不要初始化数据库了
            if (!string.IsNullOrEmpty(split))
            {
                _shardEnsured.EnsureDb(dbName, split, true).GetAwaiter().GetResult();
            }

            var connectionString = _connectionStringProvider.Get(dbName, split);

            entry = new RawConnectionContext
            {
                Connection = new MySqlConnection(connectionString)
            };

            entry.Connection.Open();

            _connections.TryAdd(key, entry);

            return entry;
        }

        public void Dispose()
        {
            foreach (var (name, connection) in _connections)
            {
                connection.Connection?.Close();
                connection.Connection?.Dispose();
            }
        }
    }
}
