using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Authorization
{
    public class AdditionalUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public AdditionalUserClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        public override async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            ClaimsPrincipal principal = await base.CreateAsync(user);

            if (principal.Identity is ClaimsIdentity identity)
            {
                identity.AddClaims(new []
                {
                    user.TwoFactorEnabled
                        ? new Claim("amr", "mfa")
                        : new Claim("amr", "pwd")
                });
            }

            return principal;
        }
    }
}
