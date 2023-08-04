using System.Security.Claims;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     token 声明类型
    /// </summary>
    public struct TokenClaimTypeNames
    {
        /// <summary>
        ///     账号唯一标识符
        /// </summary>
        public const string AccountUniqueId = "aid";

        /// <summary>
        ///     token 唯一标识符
        /// </summary>
        public const string Id = "jti";

        /// <summary>
        ///     用户名
        /// </summary>
        public const string UserName = "un";

        /// <summary>
        ///     ip 地址
        /// </summary>
        public const string Ip = "ip";

        /// <summary>
        ///     创建时间
        /// </summary>
        public const string CreateTime = "ct";

        /// <summary>
        ///     过期时间
        /// </summary>
        public const string Expires = "e";

        /// <summary>
        ///     角色
        /// </summary>
        public const string Role = ClaimTypes.Role;

        /// <summary>
        ///     是否是 refresh token
        /// </summary>
        public const string IsRefreshToken = "irt";
    }
}
