using System;
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

    [Route("availability")]
    [HttpGet]
    public async Task<ActionResult> GetAvailableLessons([FromQuery] IndividualLessonsAvailabilityRequest request)
    {
        var model = await _service.GetAvailableLessons(request.TeacherId, request.Date);
        return Ok(model);
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
            Date = request.Date.Date,
            Time = TimeSpan.FromHours(request.Hour)
        };

        var success = await _service.AddIndividualLesson(model);
        return success ? Ok() : Conflict();
    }

    [Route("cancel/{lessonId}")]
    [HttpPost]
    public async Task<ActionResult> Cancel(int lessonId)
    {
        var success = await _service.CancelIndividualLesson(User.GetId(), lessonId);
        return success ? Ok() : Conflict();
    }

    [Route("complete/{lessonId}")]
    [HttpPost]
    public async Task<ActionResult> Complete(int lessonId)
    {
        var success = await _service.CompleteIndividualLesson(User.GetId(), lessonId);
        return success ? Ok() : Conflict();
    }
}