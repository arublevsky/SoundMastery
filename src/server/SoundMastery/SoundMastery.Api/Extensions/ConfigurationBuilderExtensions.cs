using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SoundMastery.Api.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddCustomConfiguration(
            this IConfigurationBuilder configurationBuilder,
            IWebHostEnvironment hostingEnvironment)
        {
            return configurationBuilder
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", true, true)
                .AddJsonFile("appsettings.Personal.json", true, true)
                .AddEnvironmentVariables();
        }
    }
}
