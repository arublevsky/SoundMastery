using System.Collections.Generic;
using System.Linq;

namespace SoundMastery.Application.Core;

/// <summary>
/// Temp solution - later to be stored per teacher
/// </summary>
public static class WorkingHours
{
    private const int NumberOfWorkingHours = 9;

    public static IEnumerable<int> Hours => Enumerable.Range(9, NumberOfWorkingHours);
}