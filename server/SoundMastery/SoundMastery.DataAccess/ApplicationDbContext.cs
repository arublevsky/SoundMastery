using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SoundMastery.Domain;

namespace SoundMastery.Migrations
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
    }
}
