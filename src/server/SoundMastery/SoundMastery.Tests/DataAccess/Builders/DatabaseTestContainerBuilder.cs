using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.MsSql;

namespace SoundMastery.Tests.DataAccess.Builders;

public class DatabaseTestContainerBuilder
{
    public static DockerContainer Build() => new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/azure-sql-edge")
        .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(1433))
        .Build();
}