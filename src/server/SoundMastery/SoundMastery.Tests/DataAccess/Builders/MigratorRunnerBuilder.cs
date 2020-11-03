using System;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SoundMastery.DataAccess.Migrations;
using SoundMastery.DataAccess.Services;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class MigratorRunnerBuilder
    {
        private IConfiguration _configuration;

        public MigratorRunnerBuilder With(IConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public IMigrationRunner Build(DatabaseEngine engine)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration is not specified");
            }

            IServiceScope scope = CreateServiceProvider(_configuration, engine).CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        }

        private static ServiceProvider CreateServiceProvider(IConfiguration configuration, DatabaseEngine engine)
        {
            return new ServiceCollection()
                .AddSingleton<DatabaseEngineAccessor>(() => engine)
                .AddFluentMigratorCore()
                .AddSingleton<IConventionSet>(new DefaultConventionSet("SoundMastery", workingDirectory: null))
                .ConfigureRunner(builder => ConfigureMigrationRunner(configuration, engine, builder))
                .BuildServiceProvider();
        }

        private static void ConfigureMigrationRunner(IConfiguration configuration, DatabaseEngine engine,
            IMigrationRunnerBuilder runnerBuilder)
        {
            switch (engine)
            {
                case DatabaseEngine.Postgres:
                    runnerBuilder.AddPostgres()
                        .WithGlobalConnectionString(configuration.GetConnectionString("PostgresDatabaseConnection"));
                    break;
                case DatabaseEngine.SqlServer:
                    runnerBuilder.AddSqlServer()
                        .WithGlobalConnectionString(configuration.GetConnectionString("SqlServerDatabaseConnection"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}");
            }

            runnerBuilder.ScanIn(typeof(InitialMigration).Assembly).For.Migrations();
        }
    }
}
