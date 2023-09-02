using System;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Moq;
using Testcontainers.MsSql;

namespace SoundMastery.Tests.DataAccess.Builders;

public class ConfigurationBuilder
{
    private DockerContainer _container;

    public ConfigurationBuilder For(DockerContainer container)
    {
        _container = container;
        return this;
    }

    public IConfiguration Build()
    {
        if (_container == null)
        {
            throw new InvalidOperationException("Test container is not specified");
        }

        var configuration = new Mock<IConfiguration>();

        // configure ConnectionStrings settings
        configuration.Setup(config => config.GetSection("ConnectionStrings"))
            .Returns(ConfigureConnectionStringSection().Object);

        return configuration.Object;
    }

    private Mock<IConfigurationSection> ConfigureConnectionStringSection()
    {
        var cs = ((MsSqlContainer)_container).GetConnectionString().Replace("master", "soundmastery");

        var csSection = new Mock<IConfigurationSection>();
        csSection.SetupGet(p => p["SqlServerDatabaseConnection"]).Returns(cs);
        return csSection;
    }
}