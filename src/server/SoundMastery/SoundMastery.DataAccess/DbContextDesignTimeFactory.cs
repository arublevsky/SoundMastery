using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SoundMastery.Migrations
{
    /// <inheritdoc />
    public class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <inheritdoc />
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = ConfigurationFactory.Create(args);
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new ApplicationDbContext(builder.Options, configuration);
        }
    }
}
