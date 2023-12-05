using System.ComponentModel.DataAnnotations.Schema;

namespace SoundMastery.Domain.Core;

public class CourseLessonHomeAssignmentMaterial
{
    [ForeignKey(nameof(CourseLessonHomeAssignment))]
    [Column(Order = 0)]
    public int CourseHomeAssignmentId { get; set; }

    [ForeignKey(nameof(Material))]
    [Column(Order = 1)]
    public int MaterialId { get; set; }

    public virtual CourseLessonHomeAssignment CourseLessonHomeAssignment { get; set; }

    public virtual Material Material { get; set; }
}