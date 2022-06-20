using System;
using System.Threading.Tasks;
using FluentAssertions;
using SoundMastery.DataAccess.Common;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.DatabaseManagement
{
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
            var sut = new DatabaseManagerBuilder().With(configuration).Build(engine);
            await container.StartAsync();

            // Act
            Func<Task> action = () => sut.CheckConnection();

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
            var sut = new DatabaseManagerBuilder().With(configuration).Build(engine);

            // Act: container not started
            Func<Task> action = () => sut.CheckConnection();

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
            var sut = new DatabaseManagerBuilder().With(configuration).Build(engine);
            await container.StartAsync();

            // Act
            await sut.EnsureDatabaseCreated();
            await sut.Drop();

            // Assert
            (await sut.DatabaseExists()).Should().BeFalse();
        }

        [Theory]
        [InlineData(DatabaseEngine.Postgres)]
        [InlineData(DatabaseEngine.SqlServer)]
        public async Task it_ensures_that_database_is_created(DatabaseEngine engine)
        {
            // Arrange
            await using var container = new DatabaseTestContainerBuilder().Build(engine);
            var configuration = new ConfigurationBuilder().For(container).Build(engine);
            var sut = new DatabaseManagerBuilder().With(configuration).Build(engine);
            await container.StartAsync();

            // Act
            await sut.EnsureDatabaseCreated();
            await sut.Drop();
            await sut.EnsureDatabaseCreated();

            // Assert
            (await sut.DatabaseExists()).Should().BeTrue();
        }
    }
}
