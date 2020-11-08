using System;

namespace SoundMastery.Api.Controllers
{
    public class TokenAuthorizationResult
    {
        public TokenAuthorizationResult(string token, double expiresInMinutes)
        {
            Token = token;
            ExpiresInMilliseconds = TimeSpan.FromMinutes(expiresInMinutes).TotalMilliseconds;
        }

        public string Token { get; set; }

        public double ExpiresInMilliseconds { get; set; }
    }
}
