using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Users;

public class UserStore : IUserEmailStore<User>, IUserPasswordStore<User>
{
    private readonly SoundMasteryContext _context;

    public UserStore(SoundMasteryContext context)
    {
        _context = context;
    }

    public Task<string> GetUserIdAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.Id.ToString());
    }

    public Task<string> GetPasswordHashAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.PasswordHash);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken token)
    {
        return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
    }

    public Task<string> GetUserNameAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.UserName);
    }

    public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken token)
    {
        await Create(user);
        return IdentityResult.Success;
    }

    public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken token)
    {
        return FindByName(normalizedUserName)!;
    }

    public Task<string> GetEmailAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken token)
    {
        return _context.Users.Where(x => x.NormalizedEmail == normalizedEmail).SingleOrDefaultAsync(token);
    }

    public Task<string> GetNormalizedEmailAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken token)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken token)
    {
        user.PasswordHash = passwordHash;
        return Task.CompletedTask;
    }

    public Task SetEmailAsync(User user, string email, CancellationToken token)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken token)
    {
        await Update(user);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken token)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(token);
        return IdentityResult.Success;
    }

    public Task<User> FindByIdAsync(string userId, CancellationToken token)
    {
        return int.TryParse(userId, out var id)
            ? _context.Users.SingleOrDefaultAsync(x => x.Id == id, token)
            : Task.FromResult<User>(null);
    }

    public Task SetUserNameAsync(User user, string userName, CancellationToken token)
    {
        user.UserName = userName;
        return Task.CompletedTask;
    }

    public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken token)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken token)
    {
        user.EmailConfirmed = confirmed;
        _context.Users.Update(user);
        return Task.CompletedTask;
    }
    private async Task Create(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    private Task<User> FindByName(string userName)
    {
        var name = userName.ToUpperInvariant();
        return _context.Users.SingleOrDefaultAsync(x => x.NormalizedUserName == name);
    }

    private async Task Update(User user)
    {
        var existingUser = await GetUser(user.Id);
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Roles = user.Roles;

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }

    private async Task<User> GetUser(int userId)
    {
        var existingUser = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"Cannot find user to update: {userId}");
        }

        return existingUser;
    }

    public void Dispose()
    {
    }
}