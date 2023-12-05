using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SoundMastery.Domain.Core;

public class Product : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}