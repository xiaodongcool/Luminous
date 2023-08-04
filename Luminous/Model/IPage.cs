namespace LangM.AspNetCore.Model
{
    public interface IPage<T>
    {
        public IList<T> List { get; set; }
        public int Count { get; set; }
    }

    public class Page<T> : IPage<T>
    {
        public Page(IList<T> list, int count)
        {
            List = list;
            Count = count;
        }
        public IList<T> List { get; set; }
        public int Count { get; set; }
    }

    public static class Page
    {
        public static IPage<TModel> Create<TModel>(IList<TModel> list, int count)
        {
            return new Page<TModel>(list, count);
        }
    }
}
