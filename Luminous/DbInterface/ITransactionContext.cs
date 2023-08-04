namespace LangM.AspNetCore.DbInterface;

/// <summary>
///     事务上下文
/// </summary>
public interface ITransactionContext : IDisposable
{
    Task Commit();
    Task RollBack();
}