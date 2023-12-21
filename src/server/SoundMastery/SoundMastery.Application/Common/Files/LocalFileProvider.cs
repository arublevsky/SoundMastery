using System;
using System.IO;
using System.Threading.Tasks;
using SoundMastery.Application.Models;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Common;

namespace SoundMastery.Application.Common.Files;

public class LocalFileProvider : IFileProvider
{
    private const string FilesDirectory = "./files";

    private readonly IGenericRepository<FileRecord> _fileRepository;

    public LocalFileProvider(IGenericRepository<FileRecord> fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<SaveFileResult> Save(FileModel file)
    {
        var path = Path.Combine(FilesDirectory, Guid.NewGuid().ToString());
        await File.WriteAllBytesAsync(path, file.Content);
        var record = await _fileRepository.Create(new FileRecord
        {
            FileName = file.FileName,
            Reference = path,
            ContentType = file.MediaType
        });

        return new SaveFileResult(record);
    }

    public Task<FileModel> Get(int fileId)
    {
        throw new System.NotImplementedException();
    }
}