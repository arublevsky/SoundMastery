using System;
using System.IO;
using System.Threading.Tasks;
using SoundMastery.Application.Models;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Common;

namespace SoundMastery.Application.Common.Files;

public class LocalFileProvider : IFileProvider
{
    private const string FilesDirectory = "./uploads";

    private readonly IGenericRepository<FileRecord> _fileRepository;

    public LocalFileProvider(IGenericRepository<FileRecord> fileRepository)
    {
        EnsureDirectoryExists();
        _fileRepository = fileRepository;
    }

    public async Task<SaveFileResult> Save(FileModel file)
    {
        var path = Path.Combine(FilesDirectory, Guid.NewGuid().ToString());

        await using var stream = file.FileStream;
        await using (var fs = new FileStream(path, FileMode.Create))
        {
            await stream.CopyToAsync(fs);
        }

        var record = await _fileRepository.Create(new FileRecord
        {
            FileName = file.FileName,
            Reference = path,
            MediaType = file.MediaType,
            ContentType = file.MediaType
        });

        return new SaveFileResult(record);
    }

    public async Task<FileModel> Get(int fileId)
    {
        var file = await _fileRepository.Get(fileId);
        return file is not null
            ? new FileModel(file,  new FileStream(file.Reference, FileMode.Open))
            : null;
    }

    private void EnsureDirectoryExists()
    {
        if (!Directory.Exists(FilesDirectory))
        {
            Directory.CreateDirectory(FilesDirectory);
        }
    }
}