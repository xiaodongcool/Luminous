namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     打在实体上，标记使用哪个数据库
    /// </summary>
    public class DbAttribute : Attribute
    {
        public string DbName { get; set; }
        public DbAttribute(string dbName)
        {
            DbName = dbName;
        }
    }
}
