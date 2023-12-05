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
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        // Act
        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.Create(user);

        // Assert
        var result = await sut.FindByName(username);
        result.Should().BeEquivalentTo(user, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task it_should_update_a_user()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.Create(user);

        // Act
        var persistedUser = await sut.FindByName(username);

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

        await sut.Update(persistedUser);

        // Assert
        var result = await sut.FindByName(username);
        result.Should().BeEquivalentTo(persistedUser, opt => opt.Excluding(x => x.Id));
    }

    [Fact]
    public async Task it_should_add_a_refresh_token()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.Create(user);

        // Act
        var persistedUser = await sut.FindByName(username);
        await sut.AssignRefreshToken("some_token", persistedUser!.Id);

        // Assert
        var result = await sut.FindByName(username);
        result.RefreshTokens.Should().ContainSingle("some_token");
    }

    [Fact]
    public async Task it_should_delete_a_refresh_token()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new UserRepositoryBuilder().With(configuration).Build();

        const string username = "admin@gmail.com";
        var user = new UserBuilder().WithUsername(username).Build();
        await sut.Create(user);

        user = await sut.FindByName(username);
        await sut.AssignRefreshToken("some_token", user!.Id);

        // Act
        await sut.ClearRefreshToken(user!.Id);

        // Assert
        var result = await sut.FindByName(username);
        result.RefreshTokens.Should().BeEmpty();
    }
}