using System.Security.Claims;

namespace LangM.AspNetCore
{
    /// <summary>
    ///     维护 <see cref="Claim"/> 的容器
    /// </summary>
    public class ClaimContainer
    {
        private readonly Dictionary<string, string[]> _claims = new();

        /// <summary>
        ///     使用 <see cref="Claim"/> 集合初始化容器
        /// </summary>
        public ClaimContainer(IEnumerable<Claim> claims)
        {
            ArgumentChecker.ThrowIfNull(claims, nameof(claims));

            foreach (var claim in claims.GroupBy(_ => _.Type))
            {
                Set(claim.Key, claim.Select(_ => _.Value).ToArray());
            }
        }

        /// <summary>
        ///     初始化声明容器
        /// </summary>
        /// <param name="accountUniqueId">账号唯一标识符</param>
        /// <param name="userName">       用户名</param>
        /// <param name="roles">          角色</param>
        /// <param name="isRefreshToken"> 是否是 refresh token</param>
        public ClaimContainer(string accountUniqueId, string userName = "", string[] roles = null, bool isRefreshToken = false)
        {
            ArgumentChecker.ThrowIfNull(accountUniqueId, nameof(accountUniqueId));

            Set(TokenClaimTypeNames.Id, Guid.NewGuid().ToString("N"));
            Set(TokenClaimTypeNames.AccountUniqueId, accountUniqueId);
            Set(TokenClaimTypeNames.UserName, userName);
            Set(TokenClaimTypeNames.CreateTime, ParseString(DateTime.Now));

            if (isRefreshToken)
            {
                Set(TokenClaimTypeNames.IsRefreshToken, "true");
            }

            if (NotEmpty(roles))
            {
                Set(TokenClaimTypeNames.Role, roles);
            }
        }

        /// <summary>
        ///     设置声明
        /// </summary>
        public ClaimContainer Set(string type, params string[] values)
        {
            ArgumentChecker.ThrowIfNull(type, nameof(type));

            if (Empty(values))
            {
                return this;
            }

            _claims[type] = values.Distinct().ToArray();

            return this;
        }

        /// <summary>
        ///     获取声明对象
        /// </summary>
        public Claim GetClaim(string type)
        {
            ArgumentChecker.ThrowIfNull(type, nameof(type));

            if (!_claims.ContainsKey(type))
            {
                return null;
            }

            var value = _claims[type].FirstOrDefault();
            return new Claim(type, value);
        }

        /// <summary>
        ///     获取声明对象
        /// </summary>
        public Claim[] GetClaims(string type)
        {
            ArgumentChecker.ThrowIfNull(type, nameof(type));

            if (!_claims.ContainsKey(type))
            {
                return null;
            }

            return _claims[type].Select(_ => new Claim(type, _)).ToArray();
        }

        /// <summary>
        ///     获取所有声明对象
        /// </summary>
        public Claim[] GetAllClaims()
        {
            return _claims.Keys.SelectMany(GetClaims).ToArray();
        }

        /// <summary>
        ///     删除声明
        /// </summary>
        public ClaimContainer Delete(string type)
        {
            ArgumentChecker.ThrowIfNull(type, nameof(type));

            if (_claims.ContainsKey(type))
            {
                _claims.Remove(type);
            }

            return this;
        }

        /// <summary>
        ///     复制当前对象
        /// </summary>
        public ClaimContainer Copy()
        {
            return new ClaimContainer(GetAllClaims());
        }
    }
}
