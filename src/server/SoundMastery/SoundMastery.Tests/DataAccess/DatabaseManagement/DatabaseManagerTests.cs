using System;
using System.Threading.Tasks;
using FluentAssertions;
using SoundMastery.DataAccess.Common;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.DatabaseManagement;

public class DatabaseManagerTests
{
    [Theory]
    [InlineData(DatabaseEngine.Postgres)]
    [InlineData(DatabaseEngine.SqlServer)]
    public async Task it_should_check_connectivity_when_database_is_available(DatabaseEngine engine)
    {
        // Arrange
        await using var container = new DatabaseTestContainerBuilder().Build(engine);
        var configuration = new ConfigurationBuilder().For(container).Build(engine);
        var sut = new DatabaseManagerBuilder().With(configuration).Build();
        await container.StartAsync();

        // Act
        var action = () => sut.CheckConnection();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Theory]
    [InlineData(DatabaseEngine.Postgres)]
    [InlineData(DatabaseEngine.SqlServer)]
    public async Task it_should_check_connectivity_when_database_is_not_available(DatabaseEngine engine)
    {
        // Arrange
        await using var container = new DatabaseTestContainerBuilder().Build(engine);
        var configuration = new ConfigurationBuilder().For(container).Build(engine);
        var sut = new DatabaseManagerBuilder().With(configuration).Build();

        // Act: container not started
        var action = () => sut.CheckConnection();

        // Assert
        await action.Should().ThrowAsync<Exception>().WithMessage("*not*available*");
    }

    [Theory]
    [InlineData(DatabaseEngine.Postgres)]
    [InlineData(DatabaseEngine.SqlServer)]
    public async Task it_should_drop_database(DatabaseEngine engine)
    {
        // Arrange
        await using var container = new DatabaseTestContainerBuilder().Build(engine);
        var configuration = new ConfigurationBuilder().For(container).Build(engine);
        var sut = new DatabaseManagerBuilder().With(configuration).Build();
        await container.StartAsync();

        // Act
        await sut.MigrateUp();
        await sut.Drop();

        // Assert
        var func = () => sut.CheckConnection();
        await func.Should().ThrowAsync<InvalidOperationException>();
    }

    [Theory]
    [InlineData(DatabaseEngine.Postgres)]
    [InlineData(DatabaseEngine.SqlServer)]
    public async Task it_ensures_that_database_is_created(DatabaseEngine engine)
    {
        // Arrange
        await using var container = new DatabaseTestContainerBuilder().Build(engine);
        var configuration = new ConfigurationBuilder().For(container).Build(engine);
        var sut = new DatabaseManagerBuilder().With(configuration).Build();
        await container.StartAsync();

        // Act
        await sut.MigrateUp();
        await sut.Drop();
        await sut.MigrateUp();

        // Assert
        var func = () => sut.CheckConnection();
        await func.Should().NotThrowAsync();
    }
}