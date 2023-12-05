using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Core;

public class IndividualHomeAssignment : BaseEntity
{
    public int IndividualLessonId { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    public virtual IndividualLesson IndividualLesson { get; set; }

    public virtual ICollection<IndividualLessonHomeAssignmentMaterial> Materials { get; set; }

}