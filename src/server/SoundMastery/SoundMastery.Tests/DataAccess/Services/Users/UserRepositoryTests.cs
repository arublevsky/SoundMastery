using System;
using System.Threading.Tasks;
using FluentAssertions;
using SoundMastery.DataAccess.Common;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.Services.Users
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
            var user = new UserBuilder().WithUsername(username).Build();
            await sut.CreateAsync(user);

            // Assert
            var result = await sut.FindByNameAsync(username);
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
            var user = new UserBuilder().WithUsername(username).Build();
            await sut.CreateAsync(user);

            // Act
            var persistedUser = await sut.FindByNameAsync(username);

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

            await sut.UpdateAsync(persistedUser);

            // Assert
            var result = await sut.FindByNameAsync(username);
            result.Should().BeEquivalentTo(persistedUser, opt => opt.Excluding(x => x.Id));
        }

        [Theory]
        [InlineData(DatabaseEngine.Postgres)]
        [InlineData(DatabaseEngine.SqlServer)]
        public async Task it_should_add_a_refresh_token(DatabaseEngine engine)
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
            var user = new UserBuilder().WithUsername(username).Build();
            await sut.CreateAsync(user);

            // Act
            var persistedUser = await sut.FindByNameAsync(username);
            await sut.AssignRefreshToken("some_token", persistedUser!);

            // Assert
            var result = await sut.FindByNameAsync(username);
            result!.RefreshTokens.Should().ContainSingle("some_token");
        }

        [Theory]
        [InlineData(DatabaseEngine.Postgres)]
        [InlineData(DatabaseEngine.SqlServer)]
        public async Task it_should_delete_a_refresh_token(DatabaseEngine engine)
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
            var user = new UserBuilder().WithUsername(username).Build();
            await sut.CreateAsync(user);

            user = await sut.FindByNameAsync(username);
            await sut.AssignRefreshToken("some_token", user!);

            // Act
            await sut.ClearRefreshToken(user!);

            // Assert
            var result = await sut.FindByNameAsync(username);
            result!.RefreshTokens.Should().BeEmpty();
        }
    }
}
