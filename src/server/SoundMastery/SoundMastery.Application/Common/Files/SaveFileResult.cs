using SoundMastery.Domain.Common;

namespace SoundMastery.Application.Common.Files;

public class SaveFileResult
{
    public SaveFileResult(FileRecord record)
    {
        Success = true;
        FileId = record.Id;
    }

    private SaveFileResult()
    {
    }

    public bool Success { get; set; }

    public int FileId { get; set; }

    public static SaveFileResult Failure => new SaveFileResult { Success = false };
}