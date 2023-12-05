using SoundMastery.Domain.Identity;

namespace SoundMastery.Domain.Core;

public class CourseParticipant : BaseEntity
{
    public int CourseId { get; set; }

    public int UserId { get; set; }

    public virtual Course Course { get; set; }

    public virtual User User { get; set; }
}