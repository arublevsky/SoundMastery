using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
using SoundMastery.Api.Models;
using SoundMastery.Application.Core;
using SoundMastery.Application.Models;

namespace SoundMastery.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class IndividualLessonsController : ControllerBase
{
    private readonly ICoreService _service;

    public IndividualLessonsController(ICoreService service)
    {
        _service = service;
    }

    [Route("my")]
    [HttpGet]
    public async Task<ActionResult> My()
    {
        var lessons = await _service.GetMyIndividualLessons(User.GetId());
        return Ok(lessons);
    }

    [Route("add")]
    [HttpPut]
    public async Task<ActionResult> Add([FromBody] AddIndividualLessonRequest request)
    {
        var model = new AddIndividualLessonModel
        {
            TeacherId = request.TeacherId,
            StudentId = User.GetId(),
            Description = request.Description,
            StartAt = request.StartAt
        };

        var success = await _service.AddIndividualLesson(model);
        return success ? Ok() : Conflict();
    }
}