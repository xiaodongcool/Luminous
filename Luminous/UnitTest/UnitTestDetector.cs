namespace LangM.AspNetCore
{
    /// <summary>
    ///     单元测试探测
    /// </summary>
    public class UnitTestDetector
    {
        static UnitTestDetector()
        {
            var unittests = new[] { "xunit", "nunit", "mstest" };
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Select(_ => _.FullName?.ToLowerInvariant() ?? "");
            RunningFromUnitTest = assemblies.Any(assembly => unittests.Any(assembly.Contains));
        }

        /// <summary>
        ///     是否以单元测试的方式运行程序
        /// </summary>
        public static bool RunningFromUnitTest { get; }
    }
}
