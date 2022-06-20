using DotNet.Testcontainers.Containers.Modules.Abstractions;

namespace SoundMastery.Tests.Extensions
{
    public static class TestContainerDatabaseExtensions
    {
        public static string GetServerConnectionString(this TestcontainerDatabase container)
        {
            return container.ConnectionString.Replace("Database=soundmastery;", "");
        }
    }
}
