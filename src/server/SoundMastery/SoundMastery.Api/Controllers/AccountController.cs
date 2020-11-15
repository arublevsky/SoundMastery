using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Application.Authorization;

namespace SoundMastery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserAuthorizationService _authorizationService;

        public AccountController(IUserAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            TokenAuthorizationResult? result = await _authorizationService.Login(model);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentityResult result = await _authorizationService.Register(model);

            if (result.Succeeded)
            {
                return Ok(_authorizationService.GetAccessToken(model.Email!));
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [Route("refresh-token")]
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            TokenAuthorizationResult? result = await _authorizationService.RefreshToken();

            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
