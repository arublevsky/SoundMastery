using System;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using Microsoft.Extensions.Configuration;
using Moq;
using SoundMastery.DataAccess.Services;
using SoundMastery.Tests.Extensions;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class ConfigurationBuilder
    {
        private TestcontainerDatabase _container;

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
            var csSection = new Mock<IConfigurationSection>();

            if (engine == DatabaseEngine.Postgres)
            {
                csSection.SetupGet(p => p["PostgresDatabaseConnection"]).Returns(_container.ConnectionString);
                csSection.SetupGet(p => p["PostgresServerConnection"]).Returns(_container.GetServerConnectionString());
            }

            if (engine == DatabaseEngine.SqlServer)
            {
                csSection.SetupGet(p => p["SqlServerDatabaseConnection"]).Returns(_container.ConnectionString);
                csSection.SetupGet(p => p["SqlServerServerConnection"]).Returns(_container.GetServerConnectionString());
            }

            configuration.Setup(config => config.GetSection("ConnectionStrings")).Returns(csSection.Object);
            return configuration.Object;
        }
    }
}
