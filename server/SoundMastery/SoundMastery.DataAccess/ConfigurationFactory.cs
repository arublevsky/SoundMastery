using System.IO;
using Microsoft.Extensions.Configuration;

namespace SoundMastery.Migrations
{
    public static class ConfigurationFactory
    {
        public static IConfiguration Create(params string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.Personal.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            return configurationBuilder.Build();
        }
    }
}
