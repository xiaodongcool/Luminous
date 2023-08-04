namespace LangM.AspNetCore
{
    public class RedisOptions
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public int Db { get; set; }
        /// <summary>
        ///     超时时间（单位：毫秒）
        /// </summary>
        public int Timeout { get; set; }
    }
}
