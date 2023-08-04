namespace Luminous
{
    public static class ParseUtil
    {
        public static string ParseString(DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static int ParseInt(string number, int defaultValue = 0)
        {
            if (Empty(number))
            {
                return defaultValue;
            }

            if (int.TryParse(number, out var value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
