using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
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
    public async Task<ActionResult<UserProfileModel>> UpdateUserProfile([FromBody] UserModel user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        await _service.UpdateUserProfile(user);
        return Ok();
    }
}