using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.AspNetCore.Mvc
{
    public static class FilterCollectionExtensions
    {
        public static void TryAdd<T>(this FilterCollection filters) where T : IFilterMetadata
        {
            if (filters.OfType<T>().Any() == false)
            {
                filters.Add<T>();
            }
        }
    }
}
