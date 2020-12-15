using System;

namespace SoundMastery.Application.Authorization
{
    public class TokenAuthenticationResult
    {
        public TokenAuthenticationResult(string token, double expiresInMinutes)
        {
            Token = token;
            ExpiresInMilliseconds = TimeSpan.FromMinutes(expiresInMinutes).TotalMilliseconds;
        }

        public string Token { get; }

        public double ExpiresInMilliseconds { get; }
    }
}
