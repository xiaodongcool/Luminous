using System.Security.Claims;

namespace LangM.AspNetCore
{
    public interface IJwtBear
    {
        /// <summary>
        ///     创建 token
        /// </summary>
        /// <param name="claimContainer">维护 <see cref="Claim"/> 的容器</param>
        string Generate(ClaimContainer claimContainer);

        /// <summary>
        ///     读取 token
        /// </summary>
        ClaimContainer Read(string token);

        /// <summary>
        ///     验证 token 是否过期,是否被篡改
        /// </summary>
        ClaimContainer Validate(string token);
    }
}
