using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services;

public class SeedDataService : ISeedDataService
{
    private readonly IUserStore<User> _userStore;

    public SeedDataService(IUserStore<User> userStore)
    {
        _userStore = userStore;
    }

    public Task ApplySeeds(User[] users)
    {
        return Task.WhenAll(users.Select(user => _userStore.CreateAsync(user, CancellationToken.None)));
    }
}