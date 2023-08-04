using System.ComponentModel.DataAnnotations.Schema;
using SqlSugar;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     实体基类
    /// </summary>
    public abstract class FullEntity : Entity, IFullEntity
    {
        [SugarColumn(ColumnName = "delete_flag")]
        [Column("delete_flag")]
        public bool DeleteFlag { get; set; }
        [SugarColumn(ColumnName = "create_time")]
        [Column("create_time")]
        public DateTime CreateTime { get; set; }
        [SugarColumn(ColumnName = "update_time")]
        [Column("update_time")]
        public DateTime UpdateTime { get; set; }
    }
}
