using System;
using System.Linq;
using System.Threading.Tasks;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoundMastery.DataAccess.Migrations;
using SoundMastery.DataAccess.Services;
using SoundMastery.DataAccess.Services.Postgres;
using SoundMastery.DataAccess.Services.SqlServer;
using SoundMastery.DataAccess.Stores;
using SoundMastery.Domain.Identity;
using SoundMastery.Migration.Common;

namespace SoundMastery.Migration
{
    public class Program
    {
        private static IConfiguration? Configuration { get; set; }

        public static async Task Main(string[] args)
        {
            var (command, engine) = ParseParameters(args);

            var serviceProvider = CreateServices(args, engine);
            using IServiceScope scope = serviceProvider.CreateScope();
            await HandleCommand(command, scope);
        }

        private static IServiceProvider CreateServices(string[] args, DatabaseEngine engine)
        {
            Configuration = ConfigurationFactory.Create(args);

            var services = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder => ConfigureMigrationRunner(engine, runnerBuilder))
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .AddSingleton(Configuration)
                .AddSingleton<IConventionSet>(new DefaultConventionSet("SoundMastery", workingDirectory: null))
                .AddTransient<IUserStore<User>, UserStore>()
                .AddTransient<IMigrationService, MigrationService>();

            RegisterDatabaseSpecificDependencies(engine, services);

            return services.BuildServiceProvider(false);
        }

        private static void ConfigureMigrationRunner(DatabaseEngine engine, IMigrationRunnerBuilder runnerBuilder)
        {
            switch (engine)
            {
                case DatabaseEngine.Postgres:
                    runnerBuilder.AddPostgres()
                        .WithGlobalConnectionString(GetConnectionString("PostgresDatabaseConnection"));
                    break;
                case DatabaseEngine.SqlServer:
                    runnerBuilder.AddSqlServer()
                        .WithGlobalConnectionString(GetConnectionString("SqlServerDatabaseConnection"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}");
            }

            runnerBuilder.ScanIn(typeof(InitialMigration).Assembly).For.Migrations();
        }

        private static void RegisterDatabaseSpecificDependencies(DatabaseEngine engine, IServiceCollection services)
        {
            services.AddSingleton<DatabaseEngineAccessor>(() => engine);

            switch (engine)
            {
                case DatabaseEngine.Postgres:
                    services.AddTransient<IDatabaseManager, PgsqlDatabaseManager>();

                    break;
                case DatabaseEngine.SqlServer:
                    services.AddTransient<IDatabaseManager, SqlServerDatabaseManager>();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}");
            }

            services.AddTransient<IUserRepository, UserRepository>();
        }

        private static async Task HandleCommand(string command, IServiceScope scope)
        {
            var manager = scope.ServiceProvider.GetService<IDatabaseManager>();
            var migrationService = scope.ServiceProvider.GetService<IMigrationService>();

            await manager.EnsureDatabaseCreated();

            switch (command)
            {
                case "drop":
                    await manager.Drop();
                    break;
                case "recreate":
                    await manager.Drop();
                    await manager.EnsureDatabaseCreated();
                    await migrationService.MigrateUp();
                    await migrationService.ApplySeeds(SeedData.Users);
                    break;
                case "update":
                    await migrationService.MigrateUp();
                    break;
                case "check-connection":
                    await manager.CheckConnection();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, $"Unknown command {command}");
            }
        }

        private static (string command, DatabaseEngine engine) ParseParameters(string[] args)
        {
            var command = args.FirstOrDefault();
            var engine = Enum.TryParse<DatabaseEngine>(args[1], ignoreCase: true, out var result)
                ? result
                : DatabaseEngine.Postgres;

            return (command, engine);
        }

        private static string GetConnectionString(string name)
        {
            var connectionString = Configuration?.GetConnectionString(name);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Connection string {name} is not configured");
            }

            return connectionString;
        }
    }
}
