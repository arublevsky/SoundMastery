using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.DatabaseManagement;

public class DatabaseManagerTests : IAsyncLifetime
{
    private readonly DockerContainer _container = DatabaseTestContainerBuilder.Build();

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    [Fact]
    public async Task it_should_check_connectivity_when_database_is_available()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new DatabaseManagerBuilder().With(configuration).Build();

        // Act
        await sut.MigrateUp();
        var action = () => sut.CheckConnection();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task it_should_check_connectivity_when_database_is_not_available()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new DatabaseManagerBuilder().With(configuration).Build();

        // Act: DB not exists
        var action = () => sut.CheckConnection();

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("*not*available*");
    }

    [Fact]
    public async Task it_should_drop_database()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new DatabaseManagerBuilder().With(configuration).Build();

        // Act
        await sut.MigrateUp();
        await sut.Drop();

        // Assert
        var func = () => sut.CheckConnection();
        await func.Should().ThrowAsync<InvalidOperationException>();
    }
}