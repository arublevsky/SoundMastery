using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SoundMastery.Api.Extensions;

namespace SoundMastery.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var webHost = CreateWebHostBuilder(args).Build();
                ConfigureLogger(webHost);
                webHost.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging => logging.AddSerilog())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddCustomConfiguration(hostingContext.HostingEnvironment, args);
                })
                .UseStartup<Startup>()
                .UseSerilog();

        private static void ConfigureLogger(IWebHost webHost)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(webHost.Services.GetRequiredService<IConfiguration>())
                .CreateLogger();
        }
    }
}
