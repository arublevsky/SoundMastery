using System;
using SoundMastery.Domain.Core;

namespace SoundMastery.Application.Models;

public class AddIndividualLessonModel
{
    public int TeacherId { get; set; }

    public int StudentId { get; set; }

    public DateTime StartAt { get; set; }

    public string Description { get; set; }

    public IndividualLesson ToEntity() =>
        new()
        {
            TeacherId = TeacherId,
            StartAt = StartAt,
            StudentId = StudentId,
            Description = Description
        };
}