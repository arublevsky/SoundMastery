using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Domain.Core;

public class Course : BaseEntity
{
    [Required]
    [StringLength(255)]
    public string Name { get; set; }

    public int ProductId { get; set; }

    public int TeacherId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime FinishAt { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal Cost { get; set; }

    public virtual Product Product { get; set; }

    public virtual User Teacher { get; set; }

    public virtual ICollection<CourseParticipant> Participants { get; set; }

    public virtual ICollection<CourseLesson> Lessons { get; set; }
}