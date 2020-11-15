using System;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;
using Microsoft.AspNetCore.Http;
using SoundMastery.Application.Profile;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Application.Common;
using SoundMastery.Application.Identity;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace SoundMastery.Application.Authorization
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private const string RefreshTokenCookieKey = "RefreshTokenCookieKey";

        private readonly ISystemConfigurationService _configurationService;
        private readonly IUserService _userService;
        private readonly IIdentityManager _identityManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserAuthorizationService(
            ISystemConfigurationService configurationService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IIdentityManager identityManager,
            IDateTimeProvider dateTimeProvider)
        {
            _configurationService = configurationService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _identityManager = identityManager;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<TokenAuthorizationResult?> Login(LoginUserModel model)
        {
            SignInResult result =
                await _identityManager.PasswordSignInAsync(model.Username, model.Password);

            if (!result.Succeeded)
            {
                return null;
            }

            User? user = await _userService.FindByNameAsync(model.Username!);
            await SetRefreshTokenCookie(user!);
            return GetAccessToken(user!.UserName);
        }

        public async Task<TokenAuthorizationResult?> RefreshToken()
        {
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies;
            if (!cookies.TryGetValue(RefreshTokenCookieKey, out var value))
            {
                return null;
            }

            var model = JsonSerializer.Deserialize<RefreshTokenModel>(value);
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.RefreshToken))
            {
                return null;
            }

            User? user = await _userService.FindByNameAsync(model.Username);
            if (user == null || !_userService.IsValidRefreshToken(user, model.RefreshToken))
            {
                return null;
            }

            await _userService.RemoveRefreshToken(user, model.RefreshToken);
            user = (await _userService.FindByNameAsync(model.Username))!;
            await SetRefreshTokenCookie(user);

            return GetAccessToken(user.UserName);
        }

        public Task<IdentityResult> Register(RegisterUserModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                FirstName = model.FirstName!,
                LastName = model.LastName!,
                Email = model.Email,
                EmailConfirmed = true
            };

            return _identityManager.CreateAsync(user, model.Password);
        }

        public TokenAuthorizationResult GetAccessToken(string username)
        {
            var expiresIn = _configurationService.GetSetting<int>("Jwt:AccessTokenExpirationInMinutes");
            var jwtKey = _configurationService.GetSetting<string>("Jwt:Key");
            var issuer = _configurationService.GetSetting<string>("Jwt:Issuer");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(JwtRegisteredClaimNames.Email, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddMinutes(expiresIn),
                signingCredentials: credentials);

            var result = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenAuthorizationResult(result, expiresIn);
        }

        private async Task SetRefreshTokenCookie(User user)
        {
            var refreshToken = await _userService.GetOrAddRefreshToken(user);
            var expires = _configurationService.GetSetting<int>("Jwt:RefreshTokenExpirationInMinutes");
            var cookieValue = JsonSerializer.Serialize(new RefreshTokenModel
            {
                Username = user.UserName,
                RefreshToken = refreshToken
            });

            _httpContextAccessor.HttpContext.Response.Cookies.Append(RefreshTokenCookieKey, cookieValue,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = _dateTimeProvider.GetUtcNow() + TimeSpan.FromMinutes(expires)
                });
        }
    }
}
