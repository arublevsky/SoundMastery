using System;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Abstractions;
using DotNet.Testcontainers.Containers.Modules.Databases;
using SoundMastery.DataAccess.Services;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class DatabaseTestContainerBuilder
    {
        public TestcontainerDatabase Build(DatabaseEngine engine)
        {
            return engine switch
            {
                DatabaseEngine.Postgres => CreatePostgresContainer(),
                DatabaseEngine.SqlServer => CreateMsSqlContainer(),
                _ => throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}")
            };
        }

        private static TestcontainerDatabase CreateMsSqlContainer()
        {
            return new TestcontainersBuilder<MsSqlTestcontainer>()
                .WithDatabase(new MsSqlTestcontainerConfiguration
                {
                    Password = "yourStrong(!)Password",
                })
                .Build();
        }

        private static TestcontainerDatabase CreatePostgresContainer()
        {
            return new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration
                {
                    Database = "soundmastery",
                    Username = "postgres",
                    Password = "postgres",
                })
                .Build();
        }
    }
}
