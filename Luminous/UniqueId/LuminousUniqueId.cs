using SqlSugar.DistributedSystem.Snowflake;

namespace Luminous
{
    /// <summary>
    ///     雪花id
    /// </summary>
    public class LuminousUniqueId : ILuminousUniqueId
    {
        private readonly IdWorker _worker;

        public LuminousUniqueId()
        {
            _worker = new IdWorker(1, 1);
        }

        public long Next() => _worker.NextId();

        public long[] Next(int count)
        {
            var result = new long[count];

            for (var i = 0; i < count; i++)
            {
                result[i] = _worker.NextId();
            }

            return result;
        }
    }
}
