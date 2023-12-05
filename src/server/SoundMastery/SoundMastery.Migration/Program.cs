using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.DataAccess.Services;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.DataAccess.Services.Users;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;
using SoundMastery.Migration.Common;

namespace SoundMastery.Migration;

public static class Program
{
    private static IConfiguration Configuration { get; set; }

    public static async Task Main(string[] args)
    {
        using var scope = CreateServices(args).CreateScope();
        await HandleCommand(args.First(), scope);
    }

    private static IServiceProvider CreateServices(string[] args)
    {
        Configuration = ConfigurationFactory.Create(args);

        var services = new ServiceCollection()
            .AddSingleton(Configuration)
            .AddDbContext<SoundMasteryContext>()
            .AddTransient<IUserStore<User>, UserRepository>()
            .AddTransient<ISeedDataService, SeedDataService>()
            .AddTransient<ISystemConfigurationService, SystemConfigurationService>()
            .AddTransient<IDatabaseManager, DatabaseManager>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<IGenericRepository<Product>, GenericRepository<Product>>()
            .AddTransient<IGenericRepository<Course>, GenericRepository<Course>>();

        return services.BuildServiceProvider(false);
    }

    private static async Task HandleCommand(string command, IServiceScope scope)
    {
        Console.WriteLine($"Start handling command: {command}");
        var manager = scope.ServiceProvider.GetService<IDatabaseManager>()!;
        var seedDataService = scope.ServiceProvider.GetService<ISeedDataService>()!;

        switch (command)
        {
            case "drop":
                await manager.Drop();
                break;
            case "recreate":
                await manager.Drop();
                await manager.MigrateUp();
                await seedDataService.ApplySeeds(SeedData.Users);
                break;
            case "seeds":
                await manager.MigrateUp();
                await seedDataService.ApplySeeds(SeedData.Users);
                break;
            case "update":
                await manager.MigrateUp();
                break;
            case "check-connection":
                await manager.CheckConnection();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(command), command, $"Unknown command {command}");
        }
    }
}