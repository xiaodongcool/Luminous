using System.Security.Claims;

namespace Luminous
{
    public interface IUserPrincipal
    {
        /// <summary>
        ///     用户唯一标志符
        /// </summary>
        string AccountUniqueId { get; }
        /// <summary>
        ///     角色判断
        /// </summary>
        bool IsInRole(string role);
        /// <summary>
        ///     其他信息
        /// </summary>
        Claim[] Claims { get; }
    }
}
