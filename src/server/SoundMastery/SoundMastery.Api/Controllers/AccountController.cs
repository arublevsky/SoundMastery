using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoundMastery.Application.Authorization;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(CorsPolicyName.FrontendApp)]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(IConfiguration config, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _config = config;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginUserModel loginModel)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);

            if (!loginResult.Succeeded)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(loginModel.Username);

            return Ok(GetTokenResult(user.UserName));
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel registerModel)
        {
            var user = new User
            {
                UserName = registerModel.Email,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                Email = registerModel.Email
            };

            var identityResult = await _userManager.CreateAsync(user, registerModel.Password);
            if (identityResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok(GetTokenResult(user.Email));
            }

            return BadRequest(identityResult.Errors);
        }

        [Authorize]
        [Route("refresh-token")]
        [HttpPost]
        public async Task<IActionResult> RefreshToken()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(GetTokenResult(user.Email));
        }

        private TokenAuthorizationResult GetTokenResult(string userName)
        {
            var expiresIn = double.Parse(_config["Jwt:ExpirationInMinutes"]);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Email, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(expiresIn),
                signingCredentials: credentials);

            var tokenResult = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenAuthorizationResult(tokenResult, expiresIn);
        }
    }
}
