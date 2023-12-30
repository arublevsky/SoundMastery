using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Application.Common.Files;
using SoundMastery.Application.Models;

namespace SoundMastery.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly IFileProvider _fileProvider;

    public FileController(IFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    [Route("upload")]
    [HttpPost]
    public async Task<ActionResult> Upload([FromForm] IFormFile file)
    {
        var model = new FileModel
        {
            FileName = file.FileName,
            MediaType = file.ContentType,
            FileStream = file.OpenReadStream()
        };

        var result = await _fileProvider.Save(model);
        return result.Success ? Ok(result.FileId) : Conflict();
    }

    [Route("download/{fileId}")]
    [HttpGet]
    public async Task<ActionResult> Download(int fileId)
    {
        var model = await _fileProvider.Get(fileId);
        return model != null
            ? File(model.FileStream, "application/force-download", model.FileName)
            : NotFound();
    }
}