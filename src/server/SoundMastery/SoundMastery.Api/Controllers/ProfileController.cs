using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
using SoundMastery.Application.Profile;

namespace SoundMastery.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserProfileService _profileService;

        public ProfileController(IUserProfileService profileService)
        {
            _profileService = profileService;
        }

        [Route("get-profile")]
        [HttpGet]
        public async Task<ActionResult<UserProfile>> GetUserProfile(CancellationToken token)
        {
            string email = User.GetEmail();
            UserProfile profile = await _profileService.GetUserProfile(email, token);
            return Ok(profile);
        }

        [Route("save-profile")]
        [HttpPost]
        public async Task<ActionResult<UserProfile>> SaveUserProfile([FromBody] UserProfile profile, CancellationToken token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _profileService.SaveUserProfile(profile, token);
            return Ok();
        }
    }
}
