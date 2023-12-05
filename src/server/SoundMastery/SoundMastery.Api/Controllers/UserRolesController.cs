using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Models;
using SoundMastery.Application.Profile;

namespace SoundMastery.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly IUserService _service;

    public UserRolesController(IUserService service)
    {
        _service = service;
    }

    [Route("add")]
    [HttpPost]
    public async Task<ActionResult> Add([FromBody]AddUserRoleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var success = await _service.AddRole(request.UserId!.Value, request.Name);
        return success ? Ok() : Conflict();
    }
}