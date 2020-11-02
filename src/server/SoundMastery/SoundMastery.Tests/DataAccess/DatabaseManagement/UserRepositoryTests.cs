using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SoundMastery.DataAccess.Services;
using SoundMastery.Domain.Identity;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.DatabaseManagement
{
    public class UserRepositoryTests
    {
        [Theory]
        [InlineData(DatabaseEngine.Postgres)]
        [InlineData(DatabaseEngine.SqlServer)]
        public async Task it_should_create_a_user(DatabaseEngine engine)
        {
            // Arrange
            await using var container = new DatabaseTestContainerBuilder().Build(engine);
            var configuration = new ConfigurationBuilder().For(container).Build(engine);
            var manager = new DatabaseManagerBuilder().With(configuration).Build(engine);
            var runner = new MigratorRunnerBuilder().With(configuration).Build(engine);
            var sut = new UserRepositoryBuilder().With(configuration).Build(engine);

            await container.StartAsync();
            await manager.EnsureDatabaseCreated();
            runner.MigrateUp();

            // Act
            const string username = "admin@gmail.com";
            var user = new User
            {
                Id = default,
                UserName = username,
                NormalizedUserName = username.ToUpperInvariant(),
                Email = username,
                NormalizedEmail = username.ToUpperInvariant(),
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEEV/ceGE2f6GymUVfyWgT+45KabCWswea5/vO8R36iR9fs6LpbIodlXRme4nBqFgUQ==",
                FirstName = "John",
                LastName = "Smith",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await sut.CreateAsync(user, CancellationToken.None);

            // Assert
            var result = await sut.FindByNameAsync(username.ToUpperInvariant(), CancellationToken.None);
            result.Should().BeEquivalentTo(user, opt => opt.Excluding(x => x.Id));
        }
    }
}
