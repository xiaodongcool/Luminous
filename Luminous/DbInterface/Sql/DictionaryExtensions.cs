namespace LangM.AspNetCore.DbInterface
{
    public static class DictionaryExtensions
    {
        public static void Add<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<KeyValuePair<TKey, TValue>> values)
        {
            if (dictionary != null && values != null)
            {
                foreach (var value in values)
                {
                    dictionary.Add(value.Key, value.Value);
                }
            }
        }
    }
}
