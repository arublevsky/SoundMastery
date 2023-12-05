using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Domain.Core;

public class CourseLesson : BaseEntity
{
    [ForeignKey("Course")]
    public int CourseId { get; set; }

    [ForeignKey("Teacher")]
    public int TeacherId { get; set; }

    public int Number { get; set; }

    public DateTime StartAt { get; set; }

    public bool Completed { get; set; }

    [StringLength(255)]
    public string Description { get; set; }

    public virtual Course Course { get; set; }

    public virtual User Teacher { get; set; }

    public virtual ICollection<CourseLessonMaterial> Materials { get; set; }

    public virtual ICollection<CourseLessonAttendee> Attendees { get; set; }
}