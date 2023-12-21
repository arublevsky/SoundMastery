using Microsoft.EntityFrameworkCore;
using SoundMastery.Domain.Common;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;

namespace SoundMastery.DataAccess.Contexts;

public class SoundMasteryContext : DbContext
{
    private readonly ISystemConfigurationService _service;

    public SoundMasteryContext(ISystemConfigurationService service)
    {
        _service = service;
    }

    public DbSet<User> Users { get; set; }

    public DbSet<FileRecord> Files { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Material> Materials { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("SoundMastery");

        builder.Entity<IndividualLessonMaterial>()
            .HasIndex(p => new { p.IndividualLessonId, p.MaterialId }).IsUnique();

        builder.Entity<IndividualLesson>()
            .HasOne(p => p.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<User>()
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users);

        builder.Entity<User>()
            .HasMany(x => x.IndividualLessons)
            .WithOne(x => x.Student)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<User>()
            .HasOne(x => x.WorkingHours)
            .WithOne()
            .HasForeignKey<WorkingHours>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(
                _service.GetConnectionString("SqlServerDatabaseConnection"),
                b => b.MigrationsAssembly("SoundMastery.Migration"));
    }
}