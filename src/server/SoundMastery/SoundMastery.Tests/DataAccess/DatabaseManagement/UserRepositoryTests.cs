using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SoundMastery.DataAccess.Services;
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
            var user = new UserBuilder().WithUserName(username).Build();
            await sut.CreateAsync(user, CancellationToken.None);

            // Assert
            var result = await sut.FindByNameAsync(username.ToUpperInvariant(), CancellationToken.None);
            result.Should().BeEquivalentTo(user, opt => opt.Excluding(x => x.Id));
        }

        [Theory]
        [InlineData(DatabaseEngine.Postgres)]
        [InlineData(DatabaseEngine.SqlServer)]
        public async Task it_should_update_a_user(DatabaseEngine engine)
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

            const string username = "admin@gmail.com";
            var user = new UserBuilder().WithUserName(username).Build();
            await sut.CreateAsync(user, CancellationToken.None);

            // Act
            var persistedUser = await sut.FindByNameAsync(username.ToUpperInvariant(), CancellationToken.None);

            persistedUser!.FirstName = "NewFirstName";
            persistedUser.LastName = "NewLastName";
            persistedUser.PhoneNumber = "NewPhoneNumber";
            persistedUser.SecurityStamp = Guid.NewGuid().ToString();
            persistedUser.PhoneNumberConfirmed = true;
            persistedUser.EmailConfirmed = true;
            persistedUser.TwoFactorEnabled = true;
            persistedUser.LockoutEnd = new DateTimeOffset(DateTime.Today);
            persistedUser.LockoutEnabled = true;
            persistedUser.AccessFailedCount = 10;

            await sut.UpdateAsync(persistedUser, CancellationToken.None);

            // Assert
            var result = await sut.FindByNameAsync(username.ToUpperInvariant(), CancellationToken.None);
            result.Should().BeEquivalentTo(persistedUser, opt => opt.Excluding(x => x.Id));
        }
    }
}
