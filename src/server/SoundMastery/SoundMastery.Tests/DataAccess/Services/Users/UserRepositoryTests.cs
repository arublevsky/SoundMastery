using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.Services.Users;

public class UserRepositoryTests : IAsyncLifetime
{
    private readonly DockerContainer _container = DatabaseTestContainerBuilder.Build();

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    [Fact]
    public async Task it_should_create_a_user()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var manager = new DatabaseManagerBuilder().With(configuration).Build();
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        await manager.MigrateUp();

        // Act
        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.CreateAsync(user);

        // Assert
        var result = await sut.FindByNameAsync(username);
        result.Should().BeEquivalentTo(user, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task it_should_update_a_user()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var manager = new DatabaseManagerBuilder().With(configuration).Build();
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        await manager.MigrateUp();

        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.CreateAsync(user);

        // Act
        var persistedUser = await sut.FindByNameAsync(username);

        persistedUser.FirstName = "NewFirstName";
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

    [Fact]
    public async Task it_should_add_a_refresh_token()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var manager = new DatabaseManagerBuilder().With(configuration).Build();
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        await manager.MigrateUp();

        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.CreateAsync(user);

        // Act
        var persistedUser = await sut.FindByNameAsync(username);
        await sut.AssignRefreshToken("some_token", persistedUser!);

        // Assert
        var result = await sut.FindByNameAsync(username);
        result.RefreshTokens.Should().ContainSingle("some_token");
    }

    [Fact]
    public async Task it_should_delete_a_refresh_token()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var manager = new DatabaseManagerBuilder().With(configuration).Build();
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        await manager.MigrateUp();

        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.CreateAsync(user);

        user = await sut.FindByNameAsync(username);
        await sut.AssignRefreshToken("some_token", user!);

        // Act
        await sut.ClearRefreshToken(user!);

        // Assert
        var result = await sut.FindByNameAsync(username);
        result.RefreshTokens.Should().BeEmpty();
    }
}