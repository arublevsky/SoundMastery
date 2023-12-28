using System.Threading.Tasks;
using SoundMastery.Application.Models;

namespace SoundMastery.Application.Common.Files;

public interface IFileProvider
{
    Task<SaveFileResult> Save(FileModel file);

    Task<FileModel> Get(int fileId);
}