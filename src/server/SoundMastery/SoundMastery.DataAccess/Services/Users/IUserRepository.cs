using System.Threading.Tasks;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Users;

public interface IUserRepository
{
    Task CreateAsync(User user);

    Task<User> FindByNameAsync(string userName);

    Task<User> FindByEmailAsync(string email);

    Task UpdateAsync(User user);

    Task AssignRefreshToken(string token, User user);

    Task ClearRefreshToken(User user);
}