using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
using SoundMastery.Application.Authorization;
using SoundMastery.Application.Authorization.ExternalProviders;

namespace SoundMastery.Api.Controllers
{
    [Authorize]
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
            var result = await _authorizationService.Login(model);
            return result != null ? Ok(result) : BadRequest();
        }

        [AllowAnonymous]
        [Route("external-login")]
        [HttpPost]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLoginModel model)
        {
            var result = await _authorizationService.ExternalLogin(model);
            return result != null ? Ok(result) : BadRequest();
        }

        [AllowAnonymous]
        [Route("twitter-request-token")]
        [HttpGet]
        public async Task<IActionResult> GetTwitterRequestToken()
        {
            var token =  await _authorizationService.GetTwitterRequestToken();
            return token != null ? Ok(token) : BadRequest();
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

            var result = await _authorizationService.Register(model);

            return result.Succeeded
                ? Ok(_authorizationService.GetAccessToken(model.Email!))
                : BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [Route("refresh-token")]
        [HttpGet]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authorizationService.RefreshToken();
            return result != null ? Ok(result) : Unauthorized();
        }

        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authorizationService.Logout(User.GetEmail());
            return Ok();
        }
    }
}
