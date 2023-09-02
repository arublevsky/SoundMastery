using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Core;

public class Product
{
    public int Id { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }

    public List<Course> Courses { get; set; } = new();
}