using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain;

public abstract class BaseEntity : IHasId
{
    [Key]
    public int Id { get; set; }
}