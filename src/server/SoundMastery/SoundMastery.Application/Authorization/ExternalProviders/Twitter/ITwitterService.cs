using System.Threading.Tasks;

namespace SoundMastery.Application.Authorization.ExternalProviders.Twitter
{
    public interface ITwitterService : IExternalAuthProviderService
    {
        Task<string> AcquireRequestToken();
    }
}
