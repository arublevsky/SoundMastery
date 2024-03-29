using System.Threading.Tasks;
using SoundMastery.Application.Models;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Profile;

public interface IUserService
{
    Task<bool> AddRole(int userId, string role);

    Task<bool> UploadAvatar(int getId, string image);

    Task<UserProfileModel> UpdateUserProfile(UserModel user, WorkingHoursModel workingHours);

    Task<UserProfileModel> GetUserProfile(string email);

    Task<string> GetOrAddRefreshToken(User user);

    bool IsValidRefreshToken(RefreshToken token);

    Task ClearRefreshToken(int userId);

}