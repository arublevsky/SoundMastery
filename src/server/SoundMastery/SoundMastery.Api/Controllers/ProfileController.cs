using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
using SoundMastery.Application.Profile;

namespace SoundMastery.Api.Controllers
{
    // TODO check if default policy works
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
        public async Task<ActionResult<UserProfile>> GetUserProfile()
        {
            string email = User.GetEmail();
            UserProfile profile = await _service.GetUserProfile(email);
            return Ok(profile);
        }

        [Route("save-profile")]
        [HttpPost]
        public async Task<ActionResult<UserProfile>> SaveUserProfile([FromBody] UserProfile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _service.SaveUserProfile(profile);
            return Ok();
        }
    }
}
