using System.ComponentModel.DataAnnotations;
using SoundMastery.Application.Models;
using SoundMastery.Application.Profile;

namespace SoundMastery.Api.Models;

public class UpdateProfileRequest
{
    [Required]
    public UserModel User { get; set; }

    [Required]
    public WorkingHoursModel WorkingHours { get; set; }
}