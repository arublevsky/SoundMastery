using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SoundMastery.Application.Profile;

public class WorkingHoursModel
{
    [JsonConstructor]
    public WorkingHoursModel()
    {
    }

    public WorkingHoursModel(int from, int to)
    {
        From = from;
        To = to;
    }

    [Range(6, 23)]
    public int From { get; set; }

    [Range(6, 23)]
    public int To { get; set; }
}