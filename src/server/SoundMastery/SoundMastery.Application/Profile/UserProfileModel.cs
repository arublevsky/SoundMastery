using SoundMastery.Application.Models;
using SoundMastery.Domain.Identity;
using static SoundMastery.Application.Constants;

namespace SoundMastery.Application.Profile;

public class UserProfileModel
{
    public UserProfileModel(User user)
    {
        User = new UserModel(user);
        WorkingHours = user.WorkingHours != null
            ? new WorkingHoursModel(user.WorkingHours.From, user.WorkingHours.To)
            : null;
    }

    public UserModel User { get; }

    public WorkingHoursModel WorkingHours { get; set; }

    public bool IsTeacher => User.HasRole(Roles.Teacher);
}