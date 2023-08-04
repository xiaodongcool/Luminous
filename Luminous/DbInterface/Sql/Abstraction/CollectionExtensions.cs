namespace LangM.AspNetCore.DbInterface
{
    internal static class CollectionExtensions
    {
        public static void AddRange<TInput>(this ICollection<TInput> collection, IEnumerable<TInput> addCollection)
        {
            if (collection == null || addCollection == null)
            {
                return;
            }

            foreach (var item in addCollection)
            {
                collection.Add(item);
            }
        }
    }
}
