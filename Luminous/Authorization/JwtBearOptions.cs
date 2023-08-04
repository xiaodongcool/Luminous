namespace Luminous
{
    /// <summary>
    ///     token 配置
    /// </summary>
    public class JwtBearOptions
    {
        /// <summary>
        ///     token 选项
        /// </summary>
        public TokenOptions TokenOptions { get; set; }

        /// <summary>
        ///     refresh token 选项
        /// </summary>
        public TokenOptions RefreshTokenOptions { get; set; }
    }

    /// <summary>
    ///     创建一个 token 所需要的参数
    /// </summary>
    public class TokenOptions
    {
        /// <summary>
        ///     发行人
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        ///     观众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        ///     密钥
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        ///     过期时间
        /// </summary>
        public TimeSpan Expires { get; set; }
    }
}
