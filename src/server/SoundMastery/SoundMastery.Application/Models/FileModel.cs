using System.IO;
using Newtonsoft.Json;
using SoundMastery.Domain.Common;

namespace SoundMastery.Application.Models;

public class FileModel
{
    public FileModel()
    {
    }

    public FileModel(FileRecord file)
    {
        FileId = file.Id;
        FileName = file.FileName;
        MediaType = file.MediaType;
    }

    public FileModel(FileRecord file, Stream stream)
        : this(file)
    {
        FileStream = stream;
    }

    public int FileId { get; set; }

    public string FileName { get; set; }

    public string MediaType { get; set; }

    [JsonIgnore]
    public Stream FileStream { get; set; }
}