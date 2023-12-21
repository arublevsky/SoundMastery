using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundMastery.Domain.Core;

public class WorkingHours
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int From { get; set; }

    public int To { get; set; }

    public bool IsWorking(TimeSpan time) => time.Hours >= From && time.Hours <= To;

    public int[] GetAvailableHours(IEnumerable<int> bookedHours) =>
        Enumerable.Range(start: From, count: To - From + 1).Except(bookedHours).ToArray();
}