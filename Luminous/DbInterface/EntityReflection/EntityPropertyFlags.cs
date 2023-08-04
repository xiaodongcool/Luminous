namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     实体可以实现的基本字段
    /// </summary>
    [Flags]
    public enum EntityPropertyFlags
    {
        /// <summary>
        ///     实体实现 <see cref="IEntity"/>
        /// </summary>
        Id = 0x0,
        /// <summary>
        ///     实体实现 <see cref="ISoftDeleteProperties"/>
        /// </summary>
        SoftDelete = 0x1,
        /// <summary>
        ///     实体实现 <see cref="ICreateUpdateTimeProperties"/>
        /// </summary>
        CreateUpdateTime = 0x2,
        /// <summary>
        ///     实体实现 <see cref="IEnableProperties"/>
        /// </summary>
        Enable = 0x4,
        /// <summary>
        ///     实体实现 <see cref="IUserIdProperties"/>
        /// </summary>
        UserId = 0x8,
        /// <summary>
        ///     实体实现 <see cref="IUserNameProperties"/>
        /// </summary>
        UserName = 0x10
    }
}
