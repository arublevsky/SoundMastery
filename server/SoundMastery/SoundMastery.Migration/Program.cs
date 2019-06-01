using System;
using System.Linq;
using System.Threading.Tasks;
using SoundMastery.Domain;
using SoundMastery.Migrations;

namespace SoundMastery.Migration
{
    public class Program
    {
        /// <summary>
        ///     Default password for all seed users. Value: UserPass123
        /// </summary>
        private const string DefaultPassword =
            "AQAAAAEAACcQAAAAEEV/ceGE2f6GymUVfyWgT+45KabCWswea5/vO8R36iR9fs6LpbIodlXRme4nBqFgUQ==";

        public static async Task Main(string[] args)
        {
            var command = args.First();
            switch (command)
            {
                case "seeds":
                    await ApplySeeds();
                    break;
                case "drop":
                    await Drop();
                    break;
                default: throw new ArgumentException($"Unknown command {command}.");
            }
        }

        private static async Task Drop()
        {
            var factory = new DbContextDesignTimeFactory();
            using (var context = factory.CreateDbContext(new string[0]))
            {
                await context.Database.EnsureDeletedAsync();
            }
        }

        private static async Task ApplySeeds()
        {
            var factory = new DbContextDesignTimeFactory();
            using (var context = factory.CreateDbContext(new string[0]))
            {
                await context.Database.EnsureCreatedAsync();

                var admin = context.Users.FirstOrDefault(b => b.UserName == "admin@gmail.com");
                if (admin == null)
                {
                    await context.Users.AddAsync(new User
                    {
                        Id = default(Guid),
                        UserName = "admin@gmail.com",
                        EmailConfirmed = true,
                        PasswordHash = DefaultPassword
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
