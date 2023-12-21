using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Application.Models;

public class FileModel
{
    public int FileId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; }

    [Required]
    public string MediaType { get; set; }

    [Required]
    public byte[] Content { get; set; }
}