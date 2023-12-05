using Microsoft.EntityFrameworkCore;
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

    public DbSet<Role> Roles { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("SoundMastery");

        modelBuilder.Entity<IndividualLessonMaterial>()
            .HasIndex(p => new { p.IndividualLessonId, p.MaterialId }).IsUnique();

        modelBuilder.Entity<IndividualLessonHomeAssignmentMaterial>()
            .HasIndex(p => new { p.IndividualHomeAssignmentId, p.MaterialId }).IsUnique();

        modelBuilder.Entity<FollowingStudent>()
            .HasIndex(p => new { p.UserId, p.StudentId }).IsUnique();

        modelBuilder.Entity<CourseParticipant>()
            .HasIndex(p => new { p.UserId, p.CourseId }).IsUnique();

        modelBuilder.Entity<CourseLessonMaterial>()
            .HasIndex(p => new { p.CourseLessonId, p.MaterialId }).IsUnique();

        modelBuilder.Entity<CourseLessonHomeAssignmentMaterial>()
            .HasIndex(p => new { p.CourseHomeAssignmentId, p.MaterialId }).IsUnique();

        modelBuilder.Entity<CourseLessonAttendee>()
            .HasIndex(p => new { p.UserId, p.CourseLessonId }).IsUnique();

        modelBuilder.Entity<FollowingStudent>()
            .HasIndex(p => new { p.UserId, p.StudentId }).IsUnique();

        modelBuilder.Entity<FollowingStudent>()
            .HasOne(p => p.User)
            .WithMany(x => x.FollowingStudents)
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FollowingStudent>()
            .HasOne(p => p.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IndividualLesson>()
            .HasOne(p => p.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IndividualLesson>()
            .HasOne(p => p.Teacher)
            .WithMany()
            .HasForeignKey(x => x.TeacherId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseLessonAttendee>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseParticipant>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasMany(x => x.Courses)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Course>()
            .HasOne(p => p.Teacher)
            .WithMany(x => x.Courses)
            .HasForeignKey(x => x.TeacherId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.Restrict);
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