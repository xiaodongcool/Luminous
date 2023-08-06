namespace Luminous
{
    public class PageInfo<T>
    {
        public PageInfo(int total, List<T> data)
        {
            Total = total;
            Data = data;
        }

        public PageInfo(int total, T[] data)
        {
            Total = total;
            Data = data;
        }

        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
