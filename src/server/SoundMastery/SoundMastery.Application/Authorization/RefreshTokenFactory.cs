using System;
using System.Security.Cryptography;

namespace SoundMastery.Application.Authorization
{
    internal sealed class RefreshTokenFactory
    {
        public static string GenerateToken(int size = 32)
        {
            var randomNumber = new byte[size];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
