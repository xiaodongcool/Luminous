namespace LangM.AspNetCore
{
    public static class StringExtensions
    {
        public static int GetHashCodePermanent(this string @string)
        {
            unchecked
            {
                var hash1 = 5381;
                var hash2 = hash1;

                for (int i = 0; i < @string.Length && @string[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ @string[i];
                    if (i == @string.Length - 1 || @string[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ @string[i + 1];
                }

                return Math.Abs(hash1 + (hash2 * 1566083941));
            }
        }
    }
}
