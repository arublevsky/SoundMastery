using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Domain.Core;

public class IndividualLesson : BaseEntity
{
    public int TeacherId { get; set; }

    public int StudentId { get; set; }

    public int ProductId { get; set; }

    public DateTime StartAt { get; set; }

    public bool Completed { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal Cost { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    public virtual User Teacher { get; set; }

    public virtual User Student { get; set; }

    public virtual Product Product { get; set; }

    public virtual IndividualHomeAssignment HomeAssignment { get; set; }

    public virtual ICollection<IndividualLessonMaterial> Materials { get; set; }
}