using Dapper;
using LangM.AspNetCore.Cache;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     MySql for <see cref="ISharedCreator"/>
    /// </summary>
    public class MySqlSharedCreator : ISharedCreator
    {
        private readonly ILogger<MySqlSharedCreator> _logger;
        private readonly IFullRedisProxy _fullRedis;
        private readonly IServiceProvider _serviceProvider;

        public MySqlSharedCreator(ILogger<MySqlSharedCreator> logger, IServiceProvider serviceProvider, IFullRedisProxy fullRedis)
        {
            _logger = logger;
            _fullRedis = fullRedis;
            _serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        }

        private IDbConnection GetConnection(string dbName)
        {
            var dapper = _serviceProvider.GetService<IDapperUnitOfWork>();

            if (dapper != null)
            {
                return dapper.GetConnection(dbName, "").Connection;
            }

            //  TODO:SqlSugar
            //var sqlsugar = _serviceProvider.GetService<ISqlSugarUnitOfWork>();
            //if (sqlsugar != null)
            //{
            //    return sqlsugar.GetConnection(dbName, "").Ado.Connection;
            //}

            throw new ForegoneException(WebApiStatusCode.InternalServerError, "未配置数据库连接");
        }

        public async Task CreateDb(string dbName, string newDbName)
        {
            IDisposable locker = null;

            try
            {
                locker = _fullRedis.Client.AcquireLock(CacheKey.LockCreateDb(newDbName), 1000 * 5);

                if (await ExistsDb(newDbName))
                {
                    return;
                }

                var connection = GetConnection(dbName);

                dbName += "_init";

                var sql = $"CREATE DATABASE if not exists `{newDbName}` CHARACTER SET 'utf8mb4' COLLATE 'utf8mb4_general_ci'";

                await connection.ExecuteAsync(sql);

                sql = $"select table_name from information_schema.tables where table_schema = '{dbName}' group by table_schema,table_name order by table_schema";

                var tables = await connection.QueryAsync<string>(sql);

                var sqlBuilder = new StringBuilder(500);

                foreach (var table in tables)
                {
                    sqlBuilder.Append($"create table if not exists `{newDbName}`.`{table}` like `{dbName}`.`{table}`;");
                }

                await connection.ExecuteAsync(sqlBuilder.ToString());

                _fullRedis.Client.SADD(CacheKey.DbEnsured, newDbName);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                locker?.Dispose();
            }
        }

        public async Task CreateTable(string dbName, string tableName, string newTableName)
        {
            IDisposable locker = null;

            try
            {
                //  TODO:支持非 redis 锁，可以立即使用
                locker = _fullRedis.Client.AcquireLock(CacheKey.LockCreateTb(dbName, newTableName), 1000 * 5);

                if (await ExistsTable(dbName, newTableName))
                {
                    return;
                }

                var connection = GetConnection(dbName);

                var sql = $"select 1 from information_schema.tables where table_schema = '{dbName}' and table_name = '{newTableName}' limit 1";
                var query = await connection.QueryAsync<int>(sql);

                if (query.Any())
                {
                    return;
                }

                sql = $"create table if not exists `{dbName}`.`{newTableName}` like `{dbName}`.`{tableName}`";
                await connection.ExecuteAsync(sql);

                _fullRedis.Client.SADD(CacheKey.TbEnsured(dbName), newTableName);
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                locker?.Dispose();
            }
        }

        public Task<bool> ExistsDb(string dbName) => Task.FromResult(_fullRedis.Client.SISMEMBER(CacheKey.DbEnsured, dbName) > 0);
        public Task<bool> ExistsTable(string dbName, string tbName) => Task.FromResult(_fullRedis.Client.SISMEMBER(CacheKey.TbEnsured(dbName), tbName) > 0);
    }
}
