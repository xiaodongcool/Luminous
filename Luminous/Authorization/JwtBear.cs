using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Luminous
{
    public class JwtBear : IJwtBear
    {
        private readonly JwtBearOptions _tokenConfiguration;

        public JwtBear(JwtBearOptions tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        /// <summary>
        ///     获取 token 参数
        /// </summary>
        private TokenOptions GetParameters(bool isRefreshToken)
        {
            return isRefreshToken ? _tokenConfiguration.RefreshTokenOptions : _tokenConfiguration.TokenOptions;
        }

        /// <summary>
        ///     创建 token
        /// </summary>
        /// <param name="claimContainer">声明</param>
        public virtual string Generate(ClaimContainer claimContainer)
        {
            ArgumentGuard.CheckForNull(claimContainer, nameof(claimContainer));

            var isRefreshToken = claimContainer.GetClaim(TokenClaimTypeNames.IsRefreshToken)?.Value?.ToLowerInvariant() == "true";
            var parameters = GetParameters(isRefreshToken);

            var expires = DateTime.Now.Add(parameters.Expires);
            claimContainer.Set(TokenClaimTypeNames.Expires, ParseString(expires));

            var claims = claimContainer.GetAllClaims();

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(parameters.SecurityKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var header = new JwtHeader(signingCredentials);
            var payload = new JwtPayload(parameters.Issuer, parameters.Audience, claims, DateTime.Now, expires);

            var jwtSecurityToken = new JwtSecurityToken(header, payload);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

        /// <summary>
        ///     读取 token
        /// </summary>
        public virtual ClaimContainer Read(string token)
        {
            ArgumentGuard.CheckForNull(token, nameof(token));

            return Resolve(token, false);
        }

        /// <summary>
        ///     验证 token 是否过期,是否被篡改
        /// </summary>
        public virtual ClaimContainer Validate(string token)
        {
            ArgumentGuard.CheckForNull(token, nameof(token));

            return Resolve(token, true);
        }

        /// <summary>
        ///     解析 token
        /// </summary>
        /// <param name="validate">验证 token 是否过期,是否被篡改</param>
        protected virtual ClaimContainer Resolve(string token, bool validate)
        {
            Debug.Assert(NotEmpty(token));

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            if (validate)
            {
                var isRefreshToken = jwtSecurityToken.Claims.FirstOrDefault(_ => _.Type == TokenClaimTypeNames.IsRefreshToken)?.Value?.ToLowerInvariant() == "true";
                var parameters = GetParameters(isRefreshToken);
                var key = Encoding.ASCII.GetBytes(parameters.SecurityKey);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidIssuer = parameters.Issuer,
                    ValidateAudience = false,
                    ValidAudience = parameters.Audience,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                }, out var validatedToken);
            }

            return new ClaimContainer(jwtSecurityToken.Claims);
        }
    }
}
