using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using SoundMastery.Domain.Core;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.Services.Core;

public class MaterialsRepositoryTests : IAsyncLifetime
{
    private readonly DockerContainer _container = DatabaseTestContainerBuilder.Build();

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    [Fact]
    public async Task it_should_create_a_web_material()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new GenericRepositoryBuilder<Material>().With(configuration).Build();

        // Act
        var material = await sut.Create(new Material { Url = Guid.NewGuid().ToString() });

        // Assert
        var result = await sut.Get(material.Id);
        result.Should().BeEquivalentTo(material);
    }

    [Fact]
    public async Task it_should_delete_a_web_material()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new GenericRepositoryBuilder<Material>().With(configuration).Build();
        var material = await sut.Create(new Material { Url = Guid.NewGuid().ToString() });

        // Act
        await sut.Delete(material.Id);


        // Assert
        var results = await sut.Find(x => x.Id == material.Id);
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task it_should_update_a_web_material()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new GenericRepositoryBuilder<Material>().With(configuration).Build();
        var material = await sut.Create(new Material { Url = Guid.NewGuid().ToString() });

        // Act
        var newGuid = Guid.NewGuid().ToString();
        material.Url = newGuid;
        await sut.Update(material);

        // Assert
        var result = await sut.Get(material.Id);
        result.Url.Should().BeEquivalentTo(newGuid);
    }
}