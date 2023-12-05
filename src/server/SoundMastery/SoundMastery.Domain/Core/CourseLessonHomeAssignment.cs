using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoundMastery.Domain.Core;

public class CourseLessonHomeAssignment : BaseEntity
{
    [ForeignKey(nameof(CourseLesson))]
    public int CourseLessonId { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    public virtual CourseLesson CourseLesson { get; set; }

    public virtual ICollection<CourseLessonHomeAssignmentMaterial> Materials { get; set; }
}