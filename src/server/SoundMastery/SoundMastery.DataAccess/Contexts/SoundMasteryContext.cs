using System;
using Microsoft.EntityFrameworkCore;
using SoundMastery.DataAccess.Common;
using SoundMastery.DataAccess.Services;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Contexts;

public class SoundMasteryContext : DbContext
{
    private readonly IDatabaseConnectionService _service;

    public SoundMasteryContext(IDatabaseConnectionService service)
    {
        _service = service;
    }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SoundMastery");
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var (engine, connectionString) = _service.GetConnectionParameters();

        switch (engine)
        {
            case DatabaseEngine.Postgres:
                optionsBuilder.UseNpgsql(connectionString,
                    b => b.MigrationsAssembly("SoundMastery.Migration"));
                break;
            case DatabaseEngine.SqlServer:
                optionsBuilder.UseSqlServer(connectionString,
                    b => b.MigrationsAssembly("SoundMastery.Migration"));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(engine), engine.ToString());
        }
    }
}