using System;
using System.Collections.Generic;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Migration.Common;

internal static class SeedData
{
    private const string AdminUsername = "admin@gmail.com";

    public static readonly Role[] Roles =
    {
        new()
        {
            Name = "teacher",
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            NormalizedName = "teacher"
        }
    };

    public static readonly User[] Users =
    {
        new()
        {
            UserName = AdminUsername,
            NormalizedUserName = AdminUsername.ToUpperInvariant(),
            Email = AdminUsername,
            NormalizedEmail = AdminUsername.ToUpperInvariant(),
            EmailConfirmed = true,
            PasswordHash = Constants.DefaultPassword,
            FirstName = "John",
            LastName = "Smith",
            SecurityStamp = Guid.NewGuid().ToString(),
            Roles = new List<Role>
            {
                new()
                {
                    Name = "admin",
                    NormalizedName = "admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            }
        }
    };
}