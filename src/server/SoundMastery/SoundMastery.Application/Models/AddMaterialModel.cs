namespace SoundMastery.Application.Models;

public class AddMaterialModel
{
    public int UserId { get; set; }

    public int LessonId { get; set; }

    public string Description { get; set; }

    public string Url { get; set; }

    public int? FileId { get; set; }
}