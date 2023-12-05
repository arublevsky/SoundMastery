using System.ComponentModel.DataAnnotations.Schema;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Domain.Core;

public class CourseLessonAttendee : BaseEntity
{
    [ForeignKey(nameof(CourseLesson))]
    [Column(Order = 0)]
    public int CourseLessonId { get; set; }

    [ForeignKey(nameof(User))]
    [Column(Order = 1)]
    public int UserId { get; set; }

    public virtual CourseLesson CourseLesson { get; set; }

    public virtual User User { get; set; }
}