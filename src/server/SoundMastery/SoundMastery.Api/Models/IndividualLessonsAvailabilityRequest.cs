using System;

namespace SoundMastery.Api.Models;

public class IndividualLessonsAvailabilityRequest
{
    public int TeacherId { get; set; }

    public DateTime Date { get; set; }
}