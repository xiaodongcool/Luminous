namespace Luminous
{
    /// <summary>
    ///     mysql 日志选项
    /// </summary>
    /// <remarks>
    /// CREATE TABLE IF NOT EXISTS `logs` (
    ///     `Id` BIGINT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    ///     `Exception` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `LogLevel` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `Message` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `Timestamp` DATETIME DEFAULT NULL,
    ///     `ClientIp` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `ClientAgent` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `ThreadId` int NULL,
    ///     `AccountUniqueId` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `UserName` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `EnvironmentName` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `RequestPath` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `QueryString` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL,
    ///     `MachineName` TEXT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL
    /// )
    /// </remarks>
    public class MySqlLoggingOptions
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