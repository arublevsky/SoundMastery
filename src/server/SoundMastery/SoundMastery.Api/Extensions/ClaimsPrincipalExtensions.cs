using System.Linq;
using System.Security.Claims;

namespace SoundMastery.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return principal.Claims.Single(claim => claim.Type.Equals(ClaimsIdentity.DefaultNameClaimType)).Value;
        }
    }
}
