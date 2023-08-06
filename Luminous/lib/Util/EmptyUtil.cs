namespace Luminous
{
    public static class EmptyUtil
    {
        public static bool NotEmpty<T>(IEnumerable<T> array)
        {
            return !Empty(array);
        }

        public static bool Empty<T>(IEnumerable<T> array)
        {
            if (array == null)
            {
                return true;
            }

            return !array.Any();
        }

        public static bool NotEmpty(string value)
        {
            return !Empty(value);
        }

        public static bool Empty(string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}
