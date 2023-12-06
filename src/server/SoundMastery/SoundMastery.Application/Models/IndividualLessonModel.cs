using System;
using SoundMastery.Domain.Core;

namespace SoundMastery.Application.Models;

public class IndividualLessonModel
{
    public IndividualLessonModel(IndividualLesson lesson)
    {
        Id = lesson.Id;
        TeacherFullname = lesson.Teacher.FullName;
        Completed = lesson.Completed;
        StartAt = lesson.StartAt;
    }

    public bool Completed { get; set; }

    public DateTime StartAt { get; set; }

    public int Id { get; set; }

    public string TeacherFullname { get; set; }
}