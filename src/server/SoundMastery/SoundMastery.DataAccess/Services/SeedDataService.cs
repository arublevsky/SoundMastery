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

    public async Task ApplySeeds(User[] users)
    {
        foreach (var user in users)
        {
            await _userStore.CreateAsync(user, default);
        }
    }
}