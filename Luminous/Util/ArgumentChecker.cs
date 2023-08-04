namespace LangM.AspNetCore
{
    public static class ArgumentChecker
    {
        public static void ThrowIfNull(string parameter, string parameterName = "")
        {
            if (Empty(parameter))
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        public static void ThrowIfNull(object parameter, string parameterName = "")
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
