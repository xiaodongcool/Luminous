namespace Luminous
{
    /// <summary>
    ///     雪花id
    /// </summary>
    public interface ILuminousUniqueId
    {
        long Next();
        long[] Next(int count);
    }
}
