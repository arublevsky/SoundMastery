using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Serilog;
using SoundMastery.Application.Authorization;
using SoundMastery.Application.Models;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;

namespace SoundMastery.Application.Profile;

public class UserService : IUserService
{
    private readonly ISystemConfigurationService _configurationService;
    private readonly IRoleStore<Role> _rolesStore;
    private readonly IGenericRepository<User> _userRepository;

    public UserService(
        IGenericRepository<User> userRepository,
        ISystemConfigurationService configurationService,
        IRoleStore<Role> rolesStore)
    {
        _userRepository = userRepository;
        _configurationService = configurationService;
        _rolesStore = rolesStore;
    }

    public async Task<bool> AddRole(int userId, string roleName)
    {
        var role = await _rolesStore.FindByNameAsync(roleName, default);
        if (role == null)
        {
            return false;
        }

        var user = await _userRepository.Get(userId);
        user.Roles.Add(role);
        await _userRepository.Update(user);
        return true;
    }

    public Task<bool> UploadAvatar(int userId, string image)
    {
        var imageBytes = Convert.FromBase64String(image);

        const int sizeLimit = 8 * 1024 * 1024; // 8MB
        if (imageBytes.Length > sizeLimit)
        {
            Log.Information($"Image size is too big for user's avatar {userId}");
        }
    }

    public async Task<UserProfileModel> GetUserProfile(string email)
    {
        var user = await GetUser(email);
        return new UserProfileModel(user);
    }

    public async Task UpdateUserProfile(UserModel userModel)
    {
        var user = await GetUser(userModel.Email);

        user.FirstName = userModel.FirstName;
        user.LastName = userModel.LastName;

        await _userRepository.Update(user);
    }

    public Task<string> GetOrAddRefreshToken(User user)
    {
        var existing = user.RefreshTokens.SingleOrDefault(IsValidRefreshToken)?.Token;
        return string.IsNullOrEmpty(existing)
            ? CreateRefreshToken(user)
            : Task.FromResult(existing);
    }

    private async Task<string> CreateRefreshToken(User user)
    {
        var token = RefreshTokenFactory.GenerateToken();

        var existingUser = await _userRepository.Get(user.Id);

        existingUser.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = token,
            CreatedAtUtc = DateTime.UtcNow
        });

        await _userRepository.Update(existingUser);
        return token;
    }

    public bool IsValidRefreshToken(RefreshToken token)
    {
        var lifeTime = _configurationService.GetSetting<int>("Jwt:RefreshTokenExpirationInMinutes");
        return token.CreatedAtUtc + TimeSpan.FromMinutes(lifeTime) > DateTime.UtcNow;
    }

    public async Task ClearRefreshToken(int userId)
    {
        var existingUser = await _userRepository.Get(userId);
        existingUser.RefreshTokens.Clear();
        await _userRepository.Update(existingUser);
    }

    private async Task<User> GetUser(string email)
    {
        var user = (await _userRepository.Find(x => x.Email == email)).SingleOrDefault();
        if (user == null)
        {
            throw new InvalidOperationException($"Cannot find a user {email}");
        }

        return user;
    }
}