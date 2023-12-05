using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using SoundMastery.Domain.Core;
using SoundMastery.Tests.DataAccess.Builders;
using Xunit;

namespace SoundMastery.Tests.DataAccess.Services.Core;

public class ProductsRepositoryTests : IAsyncLifetime
{
    private readonly DockerContainer _container = DatabaseTestContainerBuilder.Build();

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    [Fact]
    public async Task it_should_create_a_product()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new GenericRepositoryBuilder<Product>().With(configuration).Build();

        // Act
        var product = await sut.Create( new Product { Name = Guid.NewGuid().ToString() });

        // Assert
        var result = await sut.Get(product.Id);
        result.Should().BeEquivalentTo(product);
    }

    [Fact]
    public async Task it_should_delete_a_product()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new GenericRepositoryBuilder<Product>().With(configuration).Build();
        var product = await sut.Create(new Product { Name = Guid.NewGuid().ToString() });

        // Act
        await sut.Delete(product.Id);


        // Assert
        var result = await sut.Find(t => t.Id == product.Id);
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task it_should_update_a_product()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().For(_container).Build();
        var sut = new GenericRepositoryBuilder<Product>().With(configuration).Build();
        var product = await sut.Create(new Product { Name = Guid.NewGuid().ToString() });

        // Act
        var newGuid = Guid.NewGuid().ToString();
        product.Name = newGuid;
        await sut.Update(product);


        // Assert
        var result = await sut.Get(product.Id);
        result.Name.Should().BeEquivalentTo(newGuid);
    }
}