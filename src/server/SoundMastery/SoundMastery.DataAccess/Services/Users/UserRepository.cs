using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Users;

public class UserRepository : IUserRepository
{
    private readonly SoundMasteryContext _context;

    public UserRepository(SoundMasteryContext context)
    {
        _context = context;
    }

    public async Task<User> Create(User user)
    {
        var entry = await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return entry.Entity;
    }

    public Task<User> FindByName(string userName)
    {
        var name = userName.ToUpperInvariant();
        return _context.Users.SingleOrDefaultAsync(x => x.NormalizedUserName == name);
    }

    public Task<User> FindByEmail(string email)
    {
        var normalizedEmail = email.ToUpperInvariant();
        return _context.Users.SingleOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail);
    }

    public async Task Update(User user)
    {
        var existingUser = await GetUser(user.Id);
        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }

    public async Task AssignRefreshToken(string token, User user)
    {
        var existingUser = await GetUser(user.Id);

        existingUser.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = token,
            CreatedAtUtc = DateTime.UtcNow
        });

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();
    }

    public async Task ClearRefreshToken(User user)
    {
        var existingUser = await GetUser(user.Id);
        existingUser.RefreshTokens.Clear();
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
        return FindByEmail(normalizedEmail)!;
    }

    public Task<string> GetNormalizedEmailAsync(User user, CancellationToken token)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public async Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken token)
    {
        user.NormalizedEmail = normalizedEmail;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(token);
    }

    public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken token)
    {
        user.PasswordHash = passwordHash;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(token);
    }

    public async Task SetEmailAsync(User user, string email, CancellationToken token)
    {
        user.Email = email;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(token);
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

    public async Task SetUserNameAsync(User user, string userName, CancellationToken token)
    {
        user.UserName = userName;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(token);
    }

    public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken token)
    {
        user.NormalizedUserName = normalizedName;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(token);
    }

    public async Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken token)
    {
        user.EmailConfirmed = confirmed;
        _context.Users.Update(user);
        await _context.SaveChangesAsync(token);
    }

    public void Dispose()
    {
    }
}