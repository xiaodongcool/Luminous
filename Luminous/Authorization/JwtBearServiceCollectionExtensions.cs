﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Luminous
{
    public static class JwtBearServiceCollectionExtensions
    {
        /// <summary>
        ///     添加授权 token
        /// </summary>
        public static void AddJwtBearToken(this IServiceCollection services, JwtBearOptions jwtBearOptions = null)
        {
            ArgumentGuard.CheckForNull(services, nameof(services));

            jwtBearOptions ??= LuminousConfiguration.Jwtbear;

            var builder = services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            ConfigureJwtBearerAuthentication(builder, jwtBearOptions.TokenOptions, TokenAuthenticationSchemes.Token);

            if (jwtBearOptions.RefreshTokenOptions != null)
            {
                ConfigureJwtBearerAuthentication(builder, jwtBearOptions.RefreshTokenOptions, TokenAuthenticationSchemes.RefreshToken);
            }

            services.TryAddSingleton(jwtBearOptions);
            services.TryAddTransient<IJwtBear, JwtBear>();
            services.TryAddScoped<IUserPrincipal, UserPrincipal>();
        }

        private static void ConfigureJwtBearerAuthentication(AuthenticationBuilder builder, TokenOptions tokenOptions, string scheme)
        {
            ArgumentGuard.CheckForNull(builder, nameof(builder));
            ArgumentGuard.CheckForNull(tokenOptions, nameof(tokenOptions));
            ArgumentGuard.CheckForNull(tokenOptions.SecurityKey, nameof(tokenOptions.SecurityKey));

            tokenOptions.Issuer = "dotnet";
            tokenOptions.Audience = "dotnet";

            builder.AddJwtBearer(scheme, jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                jwt.Events = new TokenEvents();
            });
        }
    }
}
