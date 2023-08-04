namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储接口
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        ///     分库后缀
        /// </summary>
        string DbSuffix { get; set; }

        /// <summary>
        ///     分表后缀
        /// </summary>
        string TbSuffix { get; set; }
    }
}