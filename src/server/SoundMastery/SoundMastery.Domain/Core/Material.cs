using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Core;

public class Material : BaseEntity
{
    [StringLength(4000)]
    public string Url { get; set; }
}