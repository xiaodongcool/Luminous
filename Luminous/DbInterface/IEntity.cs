namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     实体接口
    /// </summary>
    public interface IEntity
    {
        public long Id { get; set; }
    }

    /// <summary>
    ///     软删除
    /// </summary>
    public interface ISoftDeleteProperties
    {
        public bool DeleteFlag { get; set; }
    }

    /// <summary>
    ///     创建、更新时间
    /// </summary>
    public interface ICreateUpdateTimeProperties
    {
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    ///     启、禁用
    /// </summary>
    public interface IEnableProperties
    {
        public bool Enable { get; set; }
    }

    /// <summary>
    ///     创建、更新人Id
    /// </summary>
    public interface IUserIdProperties
    {
        public long CreateUserId { get; set; }
        public long UpdateUserId { get; set; }
    }

    /// <summary>
    ///     创建、更新人姓名
    /// </summary>
    public interface IUserNameProperties
    {
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
    }

    /// <summary>
    ///     实体接口
    /// </summary>
    public interface IFullEntity : IEntity, ISoftDeleteProperties, ICreateUpdateTimeProperties { }

    //  TODO:创建人 修改人信息
    public interface IUserEntity : IFullEntity
    {
        public long CreateUserId { get; set; }
        public long UpdateUserId { get; set; }
        public string CreateUserName { get; set; }
        public string UpdateUserName { get; set; }
    }
}
