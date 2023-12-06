using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SoundMastery.Application.Models;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Profile;

public interface IUserService
{
    Task<bool> AddRole(int userId, string role);

    Task<IReadOnlyCollection<UserModel>> Find(Expression<Func<User, bool>> filter);

    Task<UserModel> FindByNameAsync(string username);

    Task<UserProfile> GetUserProfile(string email);

    Task SaveUserProfile(UserProfile profile);

    Task<string> GetOrAddRefreshToken(UserModel user);

    bool IsValidRefreshToken(RefreshToken token);

    Task ClearRefreshToken(int userId);
}