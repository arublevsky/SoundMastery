using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Application.Authorization.ExternalProviders;

namespace SoundMastery.Application.Authorization
{
    public interface IUserAuthorizationService
    {
        Task<TokenAuthenticationResult> Login(LoginUserModel model);

        Task Logout(string username);

        Task<string> GetTwitterRequestToken();

        Task<TokenAuthenticationResult> ExternalLogin(ExternalLoginModel model);

        Task<TokenAuthenticationResult> RefreshToken();

        Task<IdentityResult> Register(RegisterUserModel model);

        TokenAuthenticationResult GetAccessToken(string username);
    }
}
