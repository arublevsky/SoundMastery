using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoundMastery.Domain;
using SoundMastery.Domain.Identity;
using SoundMastery.Migrations;

namespace SoundMastery.Migration
{
    public class Program
    {
        /// <summary>
        ///     Default password for all seed users. Value: UserPass123
        /// </summary>
        private const string DefaultPassword = "AQAAAAEAACcQAAAAEEV/ceGE2f6GymUVfyWgT+45KabCWswea5/vO8R36iR9fs6LpbIodlXRme4nBqFgUQ==";

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
                case "recreate":
                    await Recreate();
                    break;
                case "update":
                    await Update();
                    break;
                default:
                    throw new ArgumentException($"Unknown command {command}.");
            }
        }

        private static async Task Recreate()
        {
            await Drop();
            await Update();
            await ApplySeeds();

        }

        private static async Task Update()
        {
            var factory = new DbContextDesignTimeFactory();
            using (var context = factory.CreateDbContext(new string[0]))
            {
                await context.Database.MigrateAsync();
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
                    const string username = "admin@gmail.com";
                    await context.Users.AddAsync(new User
                    {
                        Id = default,
                        UserName = username,
                        NormalizedUserName = username.ToUpperInvariant(),
                        Email = username,
                        NormalizedEmail = username.ToUpperInvariant(),
                        EmailConfirmed = true,
                        PasswordHash = DefaultPassword,
                        FirstName = "Aleksey",
                        LastName = "Rublevsky",
                        SecurityStamp = Guid.NewGuid().ToString()
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
