using SoundMastery.Application.Models;
using SoundMastery.Domain.Identity;
using static SoundMastery.Application.Constants;

namespace SoundMastery.Application.Profile;

public class UserProfileModel
{
    public UserProfileModel(User user)
    {
        User = new UserModel(user);
    }

    public UserModel User { get; }

    public bool IsTeacher => User.HasRole(Roles.Teacher);
}