using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services;

public class SeedDataService : ISeedDataService
{
    private readonly IUserStore<User> _userStore;
    private readonly IRoleStore<Role> _roleStore;

    public SeedDataService(IUserStore<User> userStore, IRoleStore<Role> roleStore)
    {
        _userStore = userStore;
        _roleStore = roleStore;
    }

    public async Task ApplySeeds(User[] users, Role[] roles)
    {
        foreach (var role in roles)
        {
            await _roleStore.CreateAsync(role, default);
        }

        foreach (var role in users)
        {
            await _userStore.CreateAsync(role, default);
        }
    }
}