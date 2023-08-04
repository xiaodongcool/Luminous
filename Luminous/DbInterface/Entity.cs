using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     实体基类
    /// </summary>
    public abstract class Entity : IEntity
    {
        [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = false)]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
    }
}
