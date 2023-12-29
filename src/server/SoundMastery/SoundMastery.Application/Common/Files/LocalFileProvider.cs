using System.IO;
using System.Threading.Tasks;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Common;
using static SoundMastery.Application.Constants;

namespace SoundMastery.Application.Common.Files;

public class LocalFileProvider : FileProviderBase
{
    public LocalFileProvider(IGenericRepository<FileRecord> fileRepository)
        : base(fileRepository)
    {
        EnsureDirectoryExists();
    }

    protected override async Task SaveFile(string reference, Stream stream)
    {
        var path = Path.Combine(FileStorage.LocalUploadsDirectory, reference);
        await using var fs = new FileStream(path, FileMode.Create);
        await stream.CopyToAsync(fs);
    }

    protected override Task<Stream> GetFile(string reference)
    {
        var stream = new FileStream(Path.Combine(FileStorage.LocalUploadsDirectory, reference), FileMode.Open);
        return Task.FromResult((Stream)stream);
    }

    private static void EnsureDirectoryExists()
    {
        if (!Directory.Exists(FileStorage.LocalUploadsDirectory))
        {
            Directory.CreateDirectory(FileStorage.LocalUploadsDirectory);
        }
    }
}