using SoundMastery.Domain.Core;

namespace SoundMastery.Application.Models;

public class IndividualLessonMaterialModel
{
    public IndividualLessonMaterialModel(IndividualLessonMaterial material)
    {
        Description = material.Description;
        Url = material.Material.Url;
        File = material.Material.File != null ? new FileModel(material.Material.File) : null;
    }

    public string Description { get; set; }

    public string Url { get; set; }

    public FileModel File { get; set; }
}