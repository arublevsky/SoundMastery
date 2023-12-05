namespace SoundMastery.Domain.Core;

public class IndividualLessonHomeAssignmentMaterial : BaseEntity
{
    public int IndividualHomeAssignmentId { get; set; }

    public int MaterialId { get; set; }

    public virtual IndividualHomeAssignment IndividualHomeAssignment { get; set; }

    public virtual Material Material { get; set; }
}