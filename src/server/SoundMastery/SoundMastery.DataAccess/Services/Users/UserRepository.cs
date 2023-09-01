using System;
using System.Threading.Tasks;
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

    public async Task CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public Task<User> FindByNameAsync(string userName)
    {
        var name = userName.ToUpperInvariant();
        return _context.Users.SingleOrDefaultAsync(x => x.NormalizedUserName == name);
    }

    public Task<User> FindByEmailAsync(string email)
    {
        var normalizedEmail = email.ToUpperInvariant();
        return _context.Users.SingleOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail);
    }

    public async Task UpdateAsync(User user)
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
        var now = DateTime.UtcNow;

        existingUser.RefreshTokens.Add(new RefreshToken
        {
            UserId = user.Id,
            Token = token,
            CreatedAtUtc = now
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
}