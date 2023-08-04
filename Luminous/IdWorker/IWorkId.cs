using SqlSugar.DistributedSystem.Snowflake;

namespace Luminous
{
    /// <summary>
    ///     雪花id
    /// </summary>
    public interface IWorkId
    {
        long Next();
    }

    /// <summary>
    ///     雪花id
    /// </summary>
    public class WorkId : IWorkId
    {
        private readonly IdWorker _worker;

        public WorkId()
        {
            _worker = new IdWorker(1, 1);
        }

        public long Next() => _worker.NextId();
    }
}
