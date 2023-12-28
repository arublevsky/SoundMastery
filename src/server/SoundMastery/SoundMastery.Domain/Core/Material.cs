using System.ComponentModel.DataAnnotations;
using SoundMastery.Domain.Common;

namespace SoundMastery.Domain.Core;

public class Material : BaseEntity
{
    public MaterialType Type { get; set; }

    [StringLength(4000)]
    public string Url { get; set; }

    public int? FileId { get; set; }

    public virtual FileRecord File { get; set; }
}