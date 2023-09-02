using System;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Migration.Common;

internal static class SeedData
{
    private const string AdminUsername = "admin@gmail.com";

    public static readonly User[] Users = {
        new()
        {
            Id = 1,
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