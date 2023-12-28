using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Common;

public class FileRecord : BaseEntity
{
    public string FileName { get; set; }

    [MaxLength(100)]
    public string MediaType { get; set; }

    public string Reference { get; set; }

    public string ContentType { get; set; }
}