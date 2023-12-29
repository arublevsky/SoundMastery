using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Common;
using SoundMastery.Domain.Services;
using static SoundMastery.Application.Constants;

namespace SoundMastery.Application.Common.Files;

public class AzureFileProvider : FileProviderBase
{
    private readonly BlobContainerClient _blobContainerClient;

    public AzureFileProvider(
        IGenericRepository<FileRecord> fileRepository,
        ISystemConfigurationService systemConfiguration)
        : base(fileRepository)
    {
        var connectionString = systemConfiguration.GetConnectionString(FileStorage.AzureConnectionStringName);

        _blobContainerClient = new BlobServiceClient(connectionString)
            .GetBlobContainerClient(FileStorage.AzureBlobContainerName);
    }

    protected override Task SaveFile(string reference, Stream stream)
    {
        return _blobContainerClient.UploadBlobAsync(reference, stream);
    }

    protected override async Task<Stream> GetFile(string reference)
    {
        var client = _blobContainerClient.GetBlobClient(reference);
        var blob = await client.DownloadContentAsync();
        return blob.Value?.Content?.ToStream();
    }
}