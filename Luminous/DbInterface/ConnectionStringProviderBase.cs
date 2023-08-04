namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     数据库连接字符串基类
    /// </summary>
    public abstract class ConnectionStringProviderBase<TConnectionStringEntry> : IConnectionStringProvider where TConnectionStringEntry : class
    {
        private TConnectionStringEntry _connection;
        public abstract string Get(string dbName, string split);
        public abstract string DefaultDb { get; }

        protected TConnectionStringEntry Connection => _connection ??= CONFIGURATION.Get<TConnectionStringEntry>("connection-string");
    }
}
