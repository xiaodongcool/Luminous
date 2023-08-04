namespace LangM.AspNetCore.DbInterface
{
    public class SqlOptions
    {
        public SqlOptions()
        {
            SelectColumns = Array.Empty<string>();
            UpdateColumns = Array.Empty<string>();
            UpdateIgnoreColumns = new[]
            {
                nameof(IEntity.Id),
                nameof(ICreateUpdateTimeProperties.CreateTime),
                nameof(IUserIdProperties.CreateUserId),
                nameof(IUserNameProperties.CreateUserName)
            };
            Orders = Array.Empty<OrderColumn>();
            DbSuffix = "";
            TbSuffix = "";
        }

        /// <summary>
        ///     <see cref="ISql{T}.Select"/> 查询前 <see cref="Top"/> 条数据
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        ///     <see cref="ISql{T}.Select"/> 指定列
        /// </summary>
        public string[] SelectColumns { get; set; }

        /// <summary>
        ///     <see cref="ISql{T}.Update"/> 指定列
        /// </summary>
        public string[] UpdateColumns { get; set; }

        /// <summary>
        ///     <see cref="ISql{T}.Update"/> 忽略列
        /// </summary>
        public string[] UpdateIgnoreColumns { get; set; }

        /// <summary>
        ///     <see cref="ISql{T}.Select"/> 排序列
        /// </summary>
        public OrderColumn[] Orders { get; set; }

        /// <summary>
        ///     <see cref="ISql{T}.Select"/> 分页查询，页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     <see cref="ISql{T}.Select"/> 分页查询，页容量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     分库后缀
        /// </summary>
        public string DbSuffix { get; set; }

        /// <summary>
        ///     分表后缀
        /// </summary>
        public string TbSuffix { get; set; }
    }
}
