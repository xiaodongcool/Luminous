namespace Luminous
{
    /// <summary>
    ///     sqlserver 日志选项
    /// </summary>
    /// <remarks>
    /// CREATE TABLE [Logs] (
    /// 	[Id] [int] IDENTITY(1,1) NOT NULL,
    /// 	[Message] [nvarchar](max) NULL,
    /// 	[Level] [nvarchar](max) NULL,
    /// 	[TimeStamp] [datetime] NULL,
    /// 	[Exception] [nvarchar](max) NULL,
    /// 	[ClientIp] [nvarchar](max) NULL,
    /// 	[ClientAgent] [nvarchar](max) NULL,
    /// 	[ThreadId] [int] NULL,
    /// 	[AccountUniqueId] [nvarchar](max) NULL,
    /// 	[UserName] [nvarchar](max) NULL,
    /// 	[EnvironmentName] [nvarchar](max) NULL,
    /// 	[RequestPath] [nvarchar](max) NULL,
    /// 	[QueryString] [nvarchar](max) NULL,
    /// 	[MachineName] [nvarchar](max) NULL,
    ///    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([Id] ASC) 
    /// );
    /// </remarks>
    public class SqlServerLoggingOptions
    {
        /// <summary>
        ///     数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     表名
        /// </summary>
        public string Table { get; set; } = "logs";
    }
}