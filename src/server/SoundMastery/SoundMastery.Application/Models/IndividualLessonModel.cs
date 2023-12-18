using System;
using SoundMastery.Domain.Core;

namespace SoundMastery.Application.Models;

public class IndividualLessonModel
{
    public IndividualLessonModel(IndividualLesson lesson)
    {
        Id = lesson.Id;
        Teacher = new UserModel(lesson.Teacher);
        Student = new UserModel(lesson.Student);
        Completed = lesson.Completed;
        Description = lesson.Description;
        Date = lesson.Date;
        Hour = lesson.Time.Hours;
    }

    public string Description { get; set; }

    public bool Completed { get; set; }

    public DateTime Date { get; set; }

    public int Hour { get; set; }

    public int Id { get; set; }

    public UserModel Teacher { get; set; }

    public UserModel Student { get; set; }
}