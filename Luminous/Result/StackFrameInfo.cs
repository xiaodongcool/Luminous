namespace Luminous
{
    public class StackFrameInfo
    {
        public string FilePath { get; set; } = null!;
        public int LineNumber { get; set; }
        public string MethodName { get; set; } = null!;
        public string TypeName { get; set; } = null!;
    }
}
