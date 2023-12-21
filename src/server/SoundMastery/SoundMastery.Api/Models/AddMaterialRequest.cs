using SoundMastery.Application.Models;

namespace SoundMastery.Api.Models;

public class AddMaterialRequest
{
    public string Description { get; set; }

    public string Url { get; set; }

    public FileModel File { get; set; }
}