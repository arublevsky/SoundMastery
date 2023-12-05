using System;
using System.Linq;
using System.Threading.Tasks;
using SoundMastery.Application.Authorization;
using SoundMastery.DataAccess.Services.Users;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;

namespace SoundMastery.Application.Profile;

public class UserService : IUserService
{
    private readonly ISystemConfigurationService _configurationService;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, ISystemConfigurationService configurationService)
    {
        _userRepository = userRepository;
        _configurationService = configurationService;
    }

    public Task<User> FindByNameAsync(string username)
    {
        return _userRepository.FindByName(username);
    }

    public async Task<UserProfile> GetUserProfile(string email)
    {
        var user = await GetUser(email);
        return new UserProfile
        {
            Email = email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber
        };
    }

    public async Task SaveUserProfile(UserProfile profile)
    {
        var user = await GetUser(profile.Email);

        user.FirstName = profile.FirstName;
        user.LastName = profile.LastName;
        user.PhoneNumber = profile.PhoneNumber;

        await _userRepository.Update(user);
    }

    public Task<string> GetOrAddRefreshToken(User user)
    {
        var existing = FindActiveRefreshToken(user);
        return string.IsNullOrEmpty(existing)
            ? CreateRefreshToken(user)
            : Task.FromResult(existing);
    }

    private async Task<string> CreateRefreshToken(User user)
    {
        var token = RefreshTokenFactory.GenerateToken();
        await _userRepository.AssignRefreshToken(token, user);
        return token;
    }

    private string FindActiveRefreshToken(User user)
    {
        return user.RefreshTokens.SingleOrDefault(IsValidToken)?.Token;
    }

    public bool IsValidRefreshToken(User user, string token)
    {
        var existing = user.RefreshTokens.SingleOrDefault(t => t.Token.Equals(token));
        return existing != null && IsValidToken(existing);
    }

    public Task ClearRefreshToken(User user)
    {
        return _userRepository.ClearRefreshToken(user);
    }

    private bool IsValidToken(RefreshToken token)
    {
        var lifeTime = _configurationService.GetSetting<int>("Jwt:RefreshTokenExpirationInMinutes");
        return token.CreatedAtUtc + TimeSpan.FromMinutes(lifeTime) > DateTime.UtcNow;
    }

    private async Task<User> GetUser(string email)
    {
        var user = await _userRepository.FindByEmail(email);
        if (user == null)
        {
            throw new InvalidOperationException($"Cannot find a user {email}");
        }

        return user;
    }
}