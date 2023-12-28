using System.Threading.Tasks;
using SoundMastery.Application.Models;

namespace SoundMastery.Application.Common.Files;

public class AzureFileProvider : IFileProvider
{
    public Task<SaveFileResult> Save(FileModel file)
    {
        throw new System.NotImplementedException();
    }

    public Task<FileModel> Get(int fileId)
    {
        throw new System.NotImplementedException();
    }
}