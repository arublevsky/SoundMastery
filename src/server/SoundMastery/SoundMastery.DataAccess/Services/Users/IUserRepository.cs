using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Users;

public interface IUserRepository : IUserEmailStore<User>, IUserPasswordStore<User>
{
    Task<User> Create(User user);

    Task<User> FindByName(string userName);

    Task<User> FindByEmail(string email);

    Task Update(User user);

    Task AssignRefreshToken(string token, User user);

    Task ClearRefreshToken(User user);
}