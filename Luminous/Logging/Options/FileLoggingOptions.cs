namespace LangM.AspNetCore
{
    /// <summary>
    ///     文件日志选项
    /// </summary>
    public class FileLoggingOptions
    {
        /// <summary>
        ///     文件名(位于 \bin\langm-logs\ 目录下)
        /// </summary>
        public string File { get; set; } = "file.log";

        /// <summary>
        ///     切割隔时间
        /// </summary>
        public FileInterval Interval { get; set; } = FileInterval.Day;
    }
}