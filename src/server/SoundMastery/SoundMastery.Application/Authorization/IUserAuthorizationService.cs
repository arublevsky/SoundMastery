using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SoundMastery.Application.Authorization
{
    public interface IUserAuthorizationService
    {
        Task<TokenAuthorizationResult?> Login(LoginUserModel model);

        Task<TokenAuthorizationResult?> RefreshToken();

        Task<IdentityResult> Register(RegisterUserModel model);

        TokenAuthorizationResult GetAccessToken(string username);
    }
}
