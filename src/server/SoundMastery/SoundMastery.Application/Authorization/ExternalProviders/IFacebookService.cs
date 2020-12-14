using System.Threading.Tasks;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Authorization.ExternalProviders
{
    public interface IFacebookService
    {
        Task ValidateAccessToken(string accessToken);

        Task<User> GetUserDataFromFacebook(string accessToken);
    }
}
