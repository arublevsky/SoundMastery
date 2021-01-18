using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SoundMastery.Api.Extensions;
using SoundMastery.Application.Profile;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Api.Controllers
{
    // [Authorize(Policy = "AdminAccess")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly UrlEncoder _urlEncoder;

        public AdministrationController(UserManager<User> userManager, IUserService userService, UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _userService = userService;
            _urlEncoder = urlEncoder;
        }

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        [Route("get-users")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var claimTwoFactorEnabled = User.Claims.FirstOrDefault(t => t.Type == "amr");
            var user = await _userService.FindByNameAsync(User.GetEmail());
            var token = _userManager.GenerateTwoFactorTokenAsync(user!, TokenOptions.DefaultAuthenticatorProvider);

            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            var sharedKey = unformattedKey;
            var authenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);

            return Ok(new
            {
                claimTwoFactorEnabled,
                sharedKey,
                authenticatorUri,
                token
            });
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("StsServerIdentity"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }
    }
}
