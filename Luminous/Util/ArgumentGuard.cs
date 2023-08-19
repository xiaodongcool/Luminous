namespace Luminous
{
    public static class ArgumentGuard
    {
        public static void CheckForNull(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentNullException(argumentName, $"{argumentName} 不能为空");
            }
        }

        public static void CheckForNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName, $"{argumentName} 不能为 null");
            }
        }
    }
}
