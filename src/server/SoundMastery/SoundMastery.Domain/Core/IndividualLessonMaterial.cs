namespace SoundMastery.Domain.Core;

public class IndividualLessonMaterial : BaseEntity
{
    public int IndividualLessonId { get; set; }

    public int MaterialId { get; set; }

    public virtual IndividualLesson IndividualLesson { get; set; }

    public virtual Material Material { get; set; }
}