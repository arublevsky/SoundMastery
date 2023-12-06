using System;
using System.Collections.Generic;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Migration.Common;

internal static class SeedData
{
    private const string AdminUsername = "admin@gmail.com";
    private const string StudentUsername = "student@gmail.com";
    private const string TeacherUsername = "teacher@gmail.com";

    public static readonly Role[] Roles =
    {
        new()
        {
            Name = "admin",
            NormalizedName = "admin",
            ConcurrencyStamp = Guid.NewGuid().ToString()
        },
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
            Roles = new List<Role> { Roles[0] }
        },
        new()
        {
            UserName = TeacherUsername,
            NormalizedUserName = TeacherUsername.ToUpperInvariant(),
            Email = TeacherUsername,
            NormalizedEmail = TeacherUsername.ToUpperInvariant(),
            EmailConfirmed = true,
            PasswordHash = Constants.DefaultPassword,
            FirstName = "Teacher",
            LastName = "John",
            SecurityStamp = Guid.NewGuid().ToString(),
            Roles = new List<Role> { Roles[1] }
        },
        new()
        {
            UserName = StudentUsername,
            NormalizedUserName = StudentUsername.ToUpperInvariant(),
            Email = StudentUsername,
            NormalizedEmail = StudentUsername.ToUpperInvariant(),
            EmailConfirmed = true,
            PasswordHash = Constants.DefaultPassword,
            FirstName = "Student",
            LastName = "John",
            SecurityStamp = Guid.NewGuid().ToString(),
        }
    };
}