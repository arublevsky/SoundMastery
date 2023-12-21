using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Core;

public class IndividualLessonMaterial : BaseEntity
{
    public int IndividualLessonId { get; set; }

    public int MaterialId { get; set; }

    [MaxLength(4000)]
    public string Description { get; set; }

    public virtual IndividualLesson IndividualLesson { get; set; }

    public virtual Material Material { get; set; }
}