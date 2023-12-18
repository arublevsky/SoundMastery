using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Identity;
using SoundMastery.Application.Authorization.ExternalProviders;
using SoundMastery.Application.Authorization.ExternalProviders.Twitter;
using SoundMastery.Application.Common;
using SoundMastery.Application.Identity;
using SoundMastery.Application.Profile;
using SoundMastery.DataAccess.Services.Common;

namespace SoundMastery.Application.Authorization;

public class UserAuthorizationService : IUserAuthorizationService
{
    private const string RefreshTokenCookieKey = "RefreshTokenCookieKey";

    private readonly ISystemConfigurationService _configurationService;
    private readonly IRoleStore<Role> _roleStore;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IIdentityManager _identityManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IExternalAuthProviderResolver _authProviderResolver;
    private readonly ITwitterService _twitterService;
    private readonly IUserService _userService;

    public const string IdClaimName = "custom_claim_user_id";

    public UserAuthorizationService(
        ISystemConfigurationService configurationService,
        IHttpContextAccessor httpContextAccessor,
        IGenericRepository<User> userRepository,
        IIdentityManager identityManager,
        IDateTimeProvider dateTimeProvider,
        IExternalAuthProviderResolver authProviderResolver,
        ITwitterService twitterService,
        IRoleStore<Role> roleStore,
        IUserService userService)
    {
        _configurationService = configurationService;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _identityManager = identityManager;
        _dateTimeProvider = dateTimeProvider;
        _authProviderResolver = authProviderResolver;
        _twitterService = twitterService;
        _roleStore = roleStore;
        _userService = userService;
    }

    public async Task<TokenAuthenticationResult> Login(LoginUserModel model)
    {
        var result = await _identityManager.PasswordSignInAsync(model.Username, model.Password);
        if (!result.Succeeded)
        {
            return null;
        }

        var user = await _userRepository.Get(x => x.UserName == model.Username);
        await SetRefreshTokenCookie(user);
        return GetAccessTokenInternal(user.Id, user.UserName);
    }

    public Task<string> GetTwitterRequestToken()
    {
        return _twitterService.AcquireRequestToken();
    }

    public async Task<TokenAuthenticationResult> ExternalLogin(ExternalLoginModel model)
    {
        if (!model.Type.HasValue)
        {
            return null;
        }

        var service = _authProviderResolver.Resolve(model.Type.Value);
        var userData = await service.GetUserData(model.AccessToken);
        if (userData == null)
        {
            return null;
        }

        var user = await _userRepository.Get(x => x.Email == userData.Email)
                   // SMELL: user needs to specify its own password later on to use form login
                   ?? await CreateNewUser(userData, $"External-{Guid.NewGuid()}");

        await SetRefreshTokenCookie(user);
        return GetAccessTokenInternal(user.Id, user.UserName);
    }

    private async Task<User> CreateNewUser(User user, string password)
    {
        var result = await _identityManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(x => $"{x.Code}{x.Description}"));
            throw new InvalidOperationException($"Could not create a new user from external system. Details: {errors}");
        }

        return await _userRepository.Get(x => x.Email == user.Email);
    }

    public async Task<TokenAuthenticationResult> RefreshToken()
    {
        var cookies = _httpContextAccessor.HttpContext?.Request.Cookies;
        if (cookies == null || !cookies.TryGetValue(RefreshTokenCookieKey, out var value) ||
            string.IsNullOrEmpty(value))
        {
            return null;
        }

        var model = JsonSerializer.Deserialize<RefreshTokenModel>(value);
        if (model == null || string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.RefreshToken))
        {
            return null;
        }

        var user = await _userRepository.Get(x => x.UserName == model.Username);
        if (user == null || !user.RefreshTokens.Any())
        {
            return null;
        }

        var existingToken = user.RefreshTokens.SingleOrDefault(t => t.Token.Equals(model.RefreshToken));
        if (existingToken == null || !_userService.IsValidRefreshToken(existingToken))
        {
            return null;
        }

        await _userService.ClearRefreshToken(user.Id);
        user = await _userRepository.Get(x => x.UserName == model.Username);
        await SetRefreshTokenCookie(user);

        return GetAccessTokenInternal(user.Id, user.UserName);
    }

    public async Task<IdentityResult> Register(RegisterUserModel model)
    {
        var roleName = ResolveRole(model.Role);
        var role = await _roleStore.FindByNameAsync(roleName, default);
        if (role == null)
        {
            return IdentityResult.Failed(
                new IdentityError
                {
                    Description = $"Cannot find role: {roleName}"
                });
        }

        var user = new User
        {
            UserName = model.Email,
            FirstName = model.FirstName!,
            LastName = model.LastName!,
            Email = model.Email,
            EmailConfirmed = true,
            Roles = new List<Role> { role }
        };

        return await _identityManager.CreateAsync(user, model.Password);
    }

    public async Task<TokenAuthenticationResult> GetAccessToken(string username)
    {
        var user = (await _userRepository.Find(x => x.UserName == username)).Single();
        return GetAccessTokenInternal(user.Id, user.UserName);
    }

    private TokenAuthenticationResult GetAccessTokenInternal(int userId, string username)
    {
        var expiresIn = _configurationService.GetSetting<int>("Jwt:AccessTokenExpirationInMinutes");
        var jwtKey = _configurationService.GetSetting<string>("Jwt:Key");
        var issuer = _configurationService.GetSetting<string>("Jwt:Issuer");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, username),
            new Claim(IdClaimName, userId.ToString()),
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

    public async Task Logout(string username)
    {
        var user = await _userRepository.Get(x => x.UserName == username);
        if (user == null)
        {
            return;
        }

        await _userService.ClearRefreshToken(user.Id);
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(RefreshTokenCookieKey);
    }

    private static string ResolveRole(RegistrationRole role)
    {
        return role switch
        {
            RegistrationRole.Teacher => Constants.Roles.Teacher,
            RegistrationRole.Student => Constants.Roles.Student,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role.ToString())
        };
    }
}