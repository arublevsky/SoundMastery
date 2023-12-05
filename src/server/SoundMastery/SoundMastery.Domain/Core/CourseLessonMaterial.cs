using System.ComponentModel.DataAnnotations.Schema;

namespace SoundMastery.Domain.Core;

public class CourseLessonMaterial
{
    [ForeignKey(nameof(CourseLesson))]
    [Column(Order = 0)]
    public int CourseLessonId { get; set; }

    [ForeignKey(nameof(Material))]
    [Column(Order = 1)]
    public int MaterialId { get; set; }

    public virtual CourseLesson CourseLesson { get; set; }

    public virtual Material Material { get; set; }
}