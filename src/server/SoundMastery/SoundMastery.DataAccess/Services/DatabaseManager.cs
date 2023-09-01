using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SoundMastery.DataAccess.Contexts;

namespace SoundMastery.DataAccess.Services;

public class DatabaseManager : IDatabaseManager
{
    private readonly SoundMasteryContext _context;

    public DatabaseManager(SoundMasteryContext context)
    {
        _context = context;
    }

    public async Task CheckConnection()
    {
        if (!await _context.Database.CanConnectAsync())
        {
            throw new InvalidOperationException("Database is not available...");
        }
    }

    public Task Drop() => _context.Database.EnsureDeletedAsync();

    public Task MigrateUp() => _context.Database.GetService<IMigrator>().MigrateAsync();
}