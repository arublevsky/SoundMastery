using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Common;

public class FileRecord : BaseEntity
{
    [MaxLength(200)]
    public string FileName { get; set; }

    [MaxLength(100)]
    public string MediaType { get; set; }

    [MaxLength(400)]
    public string Reference { get; set; }

    [MaxLength(100)]
    public string ContentType { get; set; }
}