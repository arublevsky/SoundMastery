using System;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Migration.Common
{
    internal class SeedData
    {
        private const string AdminUsername = "admin@gmail.com";

        public static User[] Users = {
            new User
            {
                Id = default,
                UserName = AdminUsername,
                NormalizedUserName = AdminUsername.ToUpperInvariant(),
                Email = AdminUsername,
                NormalizedEmail = AdminUsername.ToUpperInvariant(),
                EmailConfirmed = true,
                PasswordHash = Constants.DefaultPassword,
                FirstName = "John",
                LastName = "Smith",
                SecurityStamp = Guid.NewGuid().ToString()
            }
        };
    }
}
