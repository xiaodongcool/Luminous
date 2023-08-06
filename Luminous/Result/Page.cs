namespace Luminous
{
    public class Page<T>
    {
        public Page(int total, IEnumerable<T> data)
        {
            Total = total;
            Data = data;
        }

        public int Total { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
