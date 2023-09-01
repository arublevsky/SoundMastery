using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Core;

public class Course
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }
}