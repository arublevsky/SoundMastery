using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using SoundMastery.Application.Models;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Common;

namespace SoundMastery.Application.Common.Files;

public abstract class FileProviderBase : IFileProvider
{
    private readonly IGenericRepository<FileRecord> _fileRepository;

    protected FileProviderBase(IGenericRepository<FileRecord> fileRepository)
    {
        _fileRepository = fileRepository;
    }

    public async Task<SaveFileResult> Save(FileModel file)
    {
        var fileName = Guid.NewGuid().ToString();

        try
        {
            await SaveFile(fileName, file.FileStream);
            return await CreateFileRecord(file, fileName);
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Cannot save file ({fileName}, {file.MediaType}).");
        }

        return SaveFileResult.Failure;
    }

    public async Task<FileModel> Get(int fileId)
    {
        try
        {
            var file = await _fileRepository.Get(fileId);
            var fileStream = await GetFile(file.Reference);
            if (fileStream != null)
            {
                return new FileModel(file, fileStream);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Cannot get file ({fileId}.");
        }

        return null;
    }

    protected abstract Task SaveFile(string reference, Stream stream);

    protected abstract Task<Stream> GetFile(string reference);

    private async Task<SaveFileResult> CreateFileRecord(FileModel file, string fileName)
    {
        var record = await _fileRepository.Create(
            new FileRecord
            {
                FileName = file.FileName,
                Reference = fileName,
                MediaType = file.MediaType,
                ContentType = file.MediaType
            });

        return new SaveFileResult(record);
    }
}