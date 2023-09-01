using Microsoft.EntityFrameworkCore;
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
        var connectionString = _service.GetConnectionString();
        optionsBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly("SoundMastery.Migration"));
    }
}