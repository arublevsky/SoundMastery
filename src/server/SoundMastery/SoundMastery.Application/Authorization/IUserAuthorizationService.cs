using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Application.Authorization.ExternalProviders;

namespace SoundMastery.Application.Authorization
{
    public interface IUserAuthorizationService
    {
        Task<TokenAuthorizationResult?> Login(LoginUserModel model);

        Task<TokenAuthorizationResult?> ExternalLogin(ExternalLoginModel model);

        Task<TokenAuthorizationResult?> RefreshToken();

        Task<IdentityResult> Register(RegisterUserModel model);

        TokenAuthorizationResult GetAccessToken(string username);
    }
}
