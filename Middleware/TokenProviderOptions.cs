using Microsoft.IdentityModel.Tokens;
using System;

namespace Api_ELearning.Middleware
{
    public class TokenProviderOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Path { get; set; } = "/api/accounts/token";
        public SigningCredentials SigningCredentials { get; set; }
        public TimeSpan Expires { get; set; } = TimeSpan.FromDays(1);
    }
}
