namespace SoundMastery.Domain.Common;

public class FileRecord : BaseEntity
{
    public string FileName { get; set; }

    public string Reference { get; set; }

    public string ContentType { get; set; }
}