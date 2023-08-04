using System.ComponentModel.DataAnnotations;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     分片方式
    /// </summary>
    public enum Shared
    {
        [Display(Name = "分库")]
        Db,
        [Display(Name = "分表")]
        Tb
    }
}
