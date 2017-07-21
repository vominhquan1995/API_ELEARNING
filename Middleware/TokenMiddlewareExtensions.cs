using System;
using Microsoft.AspNetCore.Builder;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Api_ELearning.Middleware
{
    public static class TokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenMiddlewareExtensions(
            this IApplicationBuilder builder)
        {
            var secret = Encoding.UTF8.GetBytes("_asduashi237r09f8aufa");
            string audience = "_sda248asdasg9024udgdfh2";
            string issuer = "_asdasjqwi34s9f7stdv87";
            var jwtOptions = new TokenProviderOptions
            {
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256)
            };
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),

                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero

            };
            builder.UseMiddleware<TokenProviderMiddleware>(Microsoft.Extensions.Options.Options.Create(jwtOptions));

            return builder.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = false,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters,
                AuthenticationScheme = "Bearer"
            });
        }
    }
}
