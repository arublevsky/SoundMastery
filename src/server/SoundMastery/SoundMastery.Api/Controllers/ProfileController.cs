using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
using SoundMastery.Api.Models;
using SoundMastery.Application.Models;
using SoundMastery.Application.Profile;

namespace SoundMastery.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IUserService _service;

    public ProfileController(IUserService service)
    {
        _service = service;
    }

    [Route("get-profile")]
    [HttpGet]
    public async Task<ActionResult<UserProfileModel>> GetUserProfile()
    {
        var email = User.GetEmail();
        var profile = await _service.GetUserProfile(email);
        return Ok(profile);
    }

    [Route("update-profile")]
    [HttpPost]
    public async Task<ActionResult<UserProfileModel>> UpdateUserProfile([FromBody] UpdateProfileRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var updatedProfile = await _service.UpdateUserProfile(request.User, request.WorkingHours);
        return Ok(updatedProfile);
    }

    [Route("upload-avatar")]
    [HttpPost]
    public async Task<ActionResult<UserProfileModel>> UploadAvatar([FromBody] UploadAvatarModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var success = await _service.UploadAvatar(User.GetId(), model.Image);
        return success ? Ok() : Conflict();
    }
}