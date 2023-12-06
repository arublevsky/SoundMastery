using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
using SoundMastery.Application.Core;

namespace SoundMastery.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TeachersController : ControllerBase
{
    private readonly ICoreService _service;

    public TeachersController(ICoreService service)
    {
        _service = service;
    }

    [Route("list")]
    [HttpGet]
    public async Task<ActionResult> List()
    {
        var teachers = await _service.GetTeachers();
        return Ok(teachers);
    }

    [Route("my")]
    [HttpGet]
    public async Task<ActionResult> My()
    {
        var teachers = await _service.GetMyTeachers(User.GetId());
        return Ok(teachers);
    }
}