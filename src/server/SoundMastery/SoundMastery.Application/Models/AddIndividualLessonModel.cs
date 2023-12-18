using System;
using SoundMastery.Domain.Core;

namespace SoundMastery.Application.Models;

public class AddIndividualLessonModel
{
    public int TeacherId { get; set; }

    public int StudentId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public string Description { get; set; }

    public IndividualLesson ToEntity() =>
        new()
        {
            TeacherId = TeacherId,
            Date = Date,
            Time = Time,
            StudentId = StudentId,
            Description = Description
        };
}