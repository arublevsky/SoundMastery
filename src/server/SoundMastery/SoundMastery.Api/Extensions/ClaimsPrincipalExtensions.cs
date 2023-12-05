using System.Linq;
using System.Security.Claims;

namespace SoundMastery.Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal principal)
    {
        return principal.Claims.Single(claim => claim.Type.Equals(ClaimsIdentity.DefaultNameClaimType)).Value;
    }

    public static int GetId(this ClaimsPrincipal principal)
    {
        return int.Parse(principal.Claims.Single(claim => claim.Type.Equals("custom_claim_user_id")).Value);
    }
}