using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}