using Api_ELearning.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api_ELearning.Middleware
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenProviderOptions _options;
        private readonly IAccountRepository _account;
        private readonly IRoleRepository _role;
        public TokenProviderMiddleware(RequestDelegate next,
            IOptions<TokenProviderOptions> options,
            IAccountRepository account,
            IRoleRepository role)
        {
            _next = next;
            _options = options.Value;
            _account = account;
            _role = role;
        }
        public Task Invoke(HttpContext httpContext, IAccountRepository account)
        {
            if (!httpContext.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(httpContext);
            }
            if (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType)
            {
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad request");
            }
            return GenerationToken(httpContext,account);
        }
        private async Task GenerationToken(HttpContext httpContext,IAccountRepository account)
        {
            var email = httpContext.Request.Form["email"];
            var password = httpContext.Request.Form["password"];
            var _account =  account.Get(email, password);
            if (_account == null)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject("Invalid account"));
                return;
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };
            var userRole = _role.Get((int)_account.RoleId);
            claims.Add(new Claim(ClaimTypes.Role, userRole.RoleName.ToString()));
            claims.Add(new Claim("roleName",userRole.RoleName.ToString()));
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(_options.Expires),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var response = new
            {
                access_token = encodedJwt,
                uid = _account.Id,
                role=_account.RoleId,
                urlAvatar=_account.UrlAvatar,
                displayName = _account.FirstName +" "+ _account.LastName,
                expires_in = (int)_options.Expires.TotalSeconds
            };
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}
