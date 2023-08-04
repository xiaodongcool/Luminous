using System.Collections.Concurrent;
using System.Diagnostics;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     确保已经分片完成
    /// </summary>
    public class ShardEnsured : IShardEnsured
    {
        private readonly ISharedCreator _sharedCreator;
        private static readonly ConcurrentDictionary<string, bool> _cache = new();

        public ShardEnsured(ISharedCreator sharedCreator)
        {
            _sharedCreator = sharedCreator;
        }

        public async Task<SharedEnsureStatus> EnsureDb(string dbName, string suffix, bool create)
        {
            //  TODO:考虑一种情况，redis重启，数据丢失怎么办

            Debug.Assert(!string.IsNullOrEmpty(dbName));
            Debug.Assert(!string.IsNullOrEmpty(suffix));

            var newDbName = dbName + suffix;

            if (_cache.TryGetValue(newDbName, out _))
            {
                return SharedEnsureStatus.Exists;
            }

            if (await _sharedCreator.ExistsDb(newDbName))
            {
                _cache.TryAdd(newDbName, true);
                return SharedEnsureStatus.Exists;
            }

            if (create)
            {
                await _sharedCreator.CreateDb(dbName, dbName + suffix);

                _cache.TryAdd(newDbName, true);
                return SharedEnsureStatus.Create;
            }
            else
            {
                return SharedEnsureStatus.NotCreate;
            }
        }

        public async Task<SharedEnsureStatus> EnsureTb(string dbName, string tbName, string suffix, bool create)
        {
            Debug.Assert(!string.IsNullOrEmpty(dbName));
            Debug.Assert(!string.IsNullOrEmpty(tbName));
            Debug.Assert(!string.IsNullOrEmpty(suffix));

            var newTableName = tbName + suffix;
            var key = dbName + newTableName;

            if (_cache.TryGetValue(key, out _))
            {
                return SharedEnsureStatus.Exists;
            }

            if (await _sharedCreator.ExistsTable(dbName, newTableName))
            {
                _cache.TryAdd(key, true);
                return SharedEnsureStatus.Exists;
            }

            if (create)
            {
                await _sharedCreator.CreateTable(dbName, tbName, tbName + suffix);

                _cache.TryAdd(key, true);
                return SharedEnsureStatus.Create;
            }
            else
            {
                return SharedEnsureStatus.NotCreate;
            }
        }
    }
}
