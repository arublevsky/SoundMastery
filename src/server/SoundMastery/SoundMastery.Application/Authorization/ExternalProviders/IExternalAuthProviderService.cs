using System.Threading.Tasks;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Authorization.ExternalProviders
{
    public interface IExternalAuthProviderService
    {
        Task<User> GetUserData(string accessToken);
    }
}
