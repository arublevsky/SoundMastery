using System;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using Microsoft.Extensions.Configuration;
using Moq;
using SoundMastery.DataAccess.Common;
using SoundMastery.Tests.Extensions;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class ConfigurationBuilder
    {
        private TestcontainerDatabase? _container;

        public ConfigurationBuilder For(TestcontainerDatabase container)
        {
            _container = container;
            return this;
        }

        public IConfiguration Build(DatabaseEngine engine)
        {
            if (_container == null)
            {
                throw new InvalidOperationException("Test container is not specified");
            }

            var configuration = new Mock<IConfiguration>();

            // configure ConnectionStrings settings
            configuration.Setup(config => config.GetSection("ConnectionStrings"))
                .Returns(ConfigureConnectionStringSection(engine).Object);

            // other settings
            configuration.SetupGet(p => p["DatabaseSettings:Engine"]).Returns(engine.ToString);

            return configuration.Object;
        }

        private Mock<IConfigurationSection> ConfigureConnectionStringSection(DatabaseEngine engine)
        {
            var csSection = new Mock<IConfigurationSection>();

            if (engine == DatabaseEngine.Postgres)
            {
                csSection.SetupGet(p => p["PostgresDatabaseConnection"]).Returns(_container!.ConnectionString);
                csSection.SetupGet(p => p["PostgresServerConnection"]).Returns(_container.GetServerConnectionString());
            }

            if (engine == DatabaseEngine.SqlServer)
            {
                csSection.SetupGet(p => p["SqlServerDatabaseConnection"]).Returns(_container!.ConnectionString);
                csSection.SetupGet(p => p["SqlServerServerConnection"]).Returns(_container.GetServerConnectionString());
            }

            return csSection;
        }
    }
}
