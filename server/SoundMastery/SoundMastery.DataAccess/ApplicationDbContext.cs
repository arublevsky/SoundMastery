using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SoundMastery.Domain;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Migrations
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        // https://github.com/aspnet/AspNetCore/issues/5793
        public DbSet<IdentityUserClaim<Guid>> IdentityUserClaims { get; set; }

        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });

            modelBuilder.Entity<User>().HasMany(u => u.Roles);

            modelBuilder.Entity<Role>().HasMany(u => u.Users);
        }
    }
}
