using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Users;

public interface IUserRepository : IUserEmailStore<User>, IUserPasswordStore<User>
{
    Task<User> Create(User user);

    Task<User> Get(int userId);

    Task<User> FindByName(string userName);

    Task<IReadOnlyCollection<User>> Find(Expression<Func<User, bool>> filter);

    Task<User> FindByEmail(string email);

    Task Update(User user);

    Task AssignRefreshToken(string token, int userId);

    Task ClearRefreshToken(int userId);
}