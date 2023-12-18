using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Users;

public sealed class RolesRepository : IRoleStore<Role>
{
    private readonly SoundMasteryContext _context;

    public RolesRepository(SoundMasteryContext context)
    {
        _context = context;
    }

    public async Task<IdentityResult> CreateAsync(Role role, CancellationToken token)
    {
        await _context.Roles.AddAsync(role, token);
        await _context.SaveChangesAsync(token);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken token)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync(token);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken token)
    {
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(token);
        return IdentityResult.Success;
    }

    public Task<string> GetRoleIdAsync(Role role, CancellationToken token)
    {
        return Task.FromResult(role.Id.ToString());
    }

    public Task<string> GetRoleNameAsync(Role role, CancellationToken token)
    {
        return Task.FromResult(role.Name);
    }

    public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken token)
    {
        role.Name = roleName;
        _context.Roles.Update(role);
        await _context.SaveChangesAsync(token);
    }

    public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken token)
    {
        return Task.FromResult(role.NormalizedName);
    }

    public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken token)
    {
        role.NormalizedName = normalizedName;
        _context.Roles.Update(role);
        await _context.SaveChangesAsync(token);
    }

    public Task<Role> FindByIdAsync(string roleId, CancellationToken token)
    {
        return int.TryParse(roleId, out var id)
            ? _context.Roles.SingleOrDefaultAsync(x => x.Id == id, token)
            : Task.FromResult<Role>(null);
    }

    public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken token)
    {
        return _context.Roles.SingleOrDefaultAsync(x => x.NormalizedName == normalizedRoleName, token);
    }

    public void Dispose()
    {
    }
}