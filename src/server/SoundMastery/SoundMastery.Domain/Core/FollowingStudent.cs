using SoundMastery.Domain.Identity;

namespace SoundMastery.Domain.Core;

public class FollowingStudent : BaseEntity
{
    public int UserId { get; set; }

    public int StudentId { get; set; }

    public virtual User User { get; set; }

    public virtual User Student { get; set; }
}