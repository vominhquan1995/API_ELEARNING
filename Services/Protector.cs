using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace Api_ELearning.Services
{
    public static class Protector
    {
        public static string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: password,
                 salt: Encoding.UTF8.GetBytes("ELear28yrawfnaosning_28yrawfnaos"),
                 prf: KeyDerivationPrf.HMACSHA1,
                 iterationCount: 10000,
                 numBytesRequested: 256 / 8
                 ));
        }
    }
}
