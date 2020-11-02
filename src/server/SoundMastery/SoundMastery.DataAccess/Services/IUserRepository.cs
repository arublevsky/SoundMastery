using System.Threading;
using System.Threading.Tasks;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services
{
    public interface IUserRepository
    {
        Task CreateAsync(User user, CancellationToken cancellationToken);

        Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);

        Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken);
    }
}
