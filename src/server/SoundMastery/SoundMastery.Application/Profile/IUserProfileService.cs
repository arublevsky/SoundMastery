using System.Threading;
using System.Threading.Tasks;

namespace SoundMastery.Application.Profile
{
    public interface IUserProfileService
    {
        Task<UserProfile> GetUserProfile(string email, CancellationToken token);

        Task SaveUserProfile(UserProfile profile, CancellationToken token);
    }
}
