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
        private readonly IExternalAuthProviderResolver _authProviderResolver;

        public UserAuthorizationService(
            ISystemConfigurationService configurationService,
            IHttpContextAccessor httpContextAccessor,
            IUserService userService,
            IIdentityManager identityManager,
            IDateTimeProvider dateTimeProvider,
            IExternalAuthProviderResolver authProviderResolver)
        {
            _configurationService = configurationService;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _identityManager = identityManager;
            _dateTimeProvider = dateTimeProvider;
            _authProviderResolver = authProviderResolver;
        }

        public async Task<TokenAuthenticationResult?> Login(LoginUserModel model)
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

        public async Task<TokenAuthenticationResult?> ExternalLogin(ExternalLoginModel model)
        {
            IExternalAuthProviderService service = _authProviderResolver.Resolve(model.Type!.Value);

            User userData = await service.GetUserData(model.AccessToken);

            User user = await _userService.FindByNameAsync(userData.Email)
                // SMELL: user needs to specify its own password later on to use form login
                ?? await CreateNewUser(userData, $"External-{Guid.NewGuid()}");

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

        public async Task<TokenAuthenticationResult?> RefreshToken()
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

        public TokenAuthenticationResult GetAccessToken(string username)
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
            return new TokenAuthenticationResult(result, expiresIn);
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
