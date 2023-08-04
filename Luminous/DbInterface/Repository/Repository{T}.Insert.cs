namespace LangM.AspNetCore.DbInterface
{
    /// <summary>
    ///     仓储抽象类
    /// </summary>
    public abstract partial class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        public abstract Task<int> Insert(T entity);

        public abstract Task<int> Insert(IList<T> entity);
    }
}
