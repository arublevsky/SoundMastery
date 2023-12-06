using System;

namespace SoundMastery.Api.Models;

public class AddIndividualLessonRequest
{
    public int TeacherId { get; set; }

    public string Description { get; set; }

    public DateTime StartAt { get; set; }
}