using System.Threading.Tasks;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Profile;

public interface IUserService
{
    Task<User> FindByNameAsync(string username);

    Task<UserProfile> GetUserProfile(string email);

    Task SaveUserProfile(UserProfile profile);

    Task<string> GetOrAddRefreshToken(User user);

    bool IsValidRefreshToken(User user, string token);

    Task ClearRefreshToken(User user);
}