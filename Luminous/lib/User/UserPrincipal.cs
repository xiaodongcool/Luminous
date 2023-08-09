using System.Security.Claims;

namespace Luminous
{
    public class UserPrincipal : IUserPrincipal
    {
        private readonly IHttpContexter _httpContextAccessor;
        private string _accountUniqueId;
        private string[] _roles;

        public UserPrincipal(IHttpContexter httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string AccountUniqueId
        {
            get
            {
                if (Empty(_accountUniqueId))
                {
                    _accountUniqueId = Claims?.FirstOrDefault(_ => _.Type == TokenClaimTypeNames.AccountUniqueId)?.Value;
                }

                return _accountUniqueId;
            }
        }

        public bool IsInRole(string role)
        {
            if (Empty(AccountUniqueId))
            {
                return false;
            }

            if (Empty(_roles))
            {
                _roles = Claims
                    ?.Where(_ => _.Type == TokenClaimTypeNames.Role)
                    ?.Select(_ => _.Value)
                    ?.Distinct()
                    ?.ToArray();
            }

            return _roles?.Contains(role) == true;
        }

        public Claim[] Claims
        {
            get
            {
                return _httpContextAccessor
                           ?.HttpContext
                           ?.User
                           ?.Claims
                           ?.ToArray()
                           ?? Array.Empty<Claim>();
            }
        }
    }
}
