using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Application.Authorization;
using SoundMastery.Application.Models;
using SoundMastery.DataAccess.Services.Users;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;

namespace SoundMastery.Application.Profile;

public class UserService : IUserService
{
    private readonly ISystemConfigurationService _configurationService;
    private readonly IRoleStore<Role> _rolesStore;
    private readonly IUserRepository _userRepository;

    public UserService(
        IUserRepository userRepository,
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

    public async Task<IReadOnlyCollection<UserModel>> Find(Expression<Func<User, bool>> filter)
    {
        return (await _userRepository.Find(filter)).Select(user => new UserModel(user)).ToArray();
    }

    public async Task<UserModel> FindByNameAsync(string username)
    {
        var user = await _userRepository.FindByName(username);
        return new UserModel(user);
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

    public Task<string> GetOrAddRefreshToken(UserModel user)
    {
        var existing = FindActiveRefreshToken(user);
        return string.IsNullOrEmpty(existing)
            ? CreateRefreshToken(user)
            : Task.FromResult(existing);
    }

    private async Task<string> CreateRefreshToken(UserModel user)
    {
        var token = RefreshTokenFactory.GenerateToken();
        await _userRepository.AssignRefreshToken(token, user.Id);
        return token;
    }

    private string FindActiveRefreshToken(UserModel user)
    {
        return user.RefreshTokens.SingleOrDefault(IsValidRefreshToken)?.Token;
    }

    public bool IsValidRefreshToken(RefreshToken token)
    {
        var lifeTime = _configurationService.GetSetting<int>("Jwt:RefreshTokenExpirationInMinutes");
        return token.CreatedAtUtc + TimeSpan.FromMinutes(lifeTime) > DateTime.UtcNow;
    }

    public Task ClearRefreshToken(int userId)
    {
        return _userRepository.ClearRefreshToken(userId);
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