using System;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;
using Microsoft.AspNetCore.Http;
using SoundMastery.Application.Profile;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Application.Authorization.ExternalProviders;
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
        private readonly IFacebookService _facebookService;

        public UserAuthorizationService(
            ISystemConfigurationService configurationService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IIdentityManager identityManager,
            IDateTimeProvider dateTimeProvider,
            IFacebookService facebookService)
        {
            _configurationService = configurationService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _identityManager = identityManager;
            _dateTimeProvider = dateTimeProvider;
            _facebookService = facebookService;
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

        public async Task<TokenAuthorizationResult?> ExternalLogin(ExternalLoginModel model)
        {
            await _facebookService.ValidateAccessToken(model.AccessToken);

            var userData = await _facebookService.GetUserDataFromFacebook(model.AccessToken);

            // SMELL: user needs to specify its own password later on to use form login
            var externalPassword = $"External-{Guid.NewGuid()}";

            User user = await _userService.FindByNameAsync(userData.Email)
                ?? await CreateNewUser(userData, externalPassword);

            await SetRefreshTokenCookie(user);
            return GetAccessToken(user.UserName);
        }

        private async Task<User> CreateNewUser(User user, string password)
        {
            var result = await _identityManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(x => $"{x.Code}{x.Description}"));
                throw new InvalidOperationException($"Could not create a new user from external system. Details: {errors}");
            }

            return (await _userService.FindByNameAsync(user.Email))!;
        }

        public async Task<TokenAuthorizationResult?> RefreshToken()
        {
            var cookies = _httpContextAccessor.HttpContext?.Request.Cookies;
            if (cookies == null || !cookies.TryGetValue(RefreshTokenCookieKey, out var value) || string.IsNullOrEmpty(value))
            {
                return null;
            }

            var model = JsonSerializer.Deserialize<RefreshTokenModel>(value);
            if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.RefreshToken))
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

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(RefreshTokenCookieKey, cookieValue,
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
