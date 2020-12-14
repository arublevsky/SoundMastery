using System;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class UserBuilder
    {
        private string _username = "admin@admin.com";

        public UserBuilder WithUsername(string userName)
        {
            _username = userName;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Id = default,
                UserName = _username,
                NormalizedUserName = _username.ToUpperInvariant(),
                Email = _username,
                NormalizedEmail = _username.ToUpperInvariant(),
                PasswordHash = "AQAAAAEAACcQAAAAEEV/ceGE2f6GymUVfyWgT+45KabCWswea5/vO8R36iR9fs6LpbIodlXRme4nBqFgUQ==",
                FirstName = "John",
                LastName = "Smith",
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }
    }
}
