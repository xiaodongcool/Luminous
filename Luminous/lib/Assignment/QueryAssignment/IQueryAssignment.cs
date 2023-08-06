namespace Luminous
{
    /// <summary>
    ///     查询条件赋值器
    /// </summary>
    public interface IQueryAssignment : IAssignment
    {
        public IQueryCollection Condition { get; }
    }
}
