using System;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class UserBuilder
    {
        private string _userName = "admin@admin.com";

        public UserBuilder WithUserName(string userName)
        {
            _userName = userName;
            return this;
        }

        public User Build()
        {
            return new User
            {
                Id = default,
                UserName = _userName,
                NormalizedUserName = _userName.ToUpperInvariant(),
                Email = _userName,
                NormalizedEmail = _userName.ToUpperInvariant(),
                PasswordHash = "AQAAAAEAACcQAAAAEEV/ceGE2f6GymUVfyWgT+45KabCWswea5/vO8R36iR9fs6LpbIodlXRme4nBqFgUQ==",
                FirstName = "John",
                LastName = "Smith",
                SecurityStamp = Guid.NewGuid().ToString()
            };
        }
    }
}
