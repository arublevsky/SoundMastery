using System;
using System.Data.Common;
using System.Data.SqlClient;
using Npgsql;
using SoundMastery.DataAccess.Common;
using SoundMastery.Domain.Services;

namespace SoundMastery.DataAccess.Services.Common
{
    public class DatabaseConnectionService : IDatabaseConnectionService
    {
        private readonly ISystemConfigurationService _configurationService;

        public DatabaseConnectionService(ISystemConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public string GetSqlPath(string name, string nestedPath, bool useEngineSpecificPath = false)
        {
            string path = useEngineSpecificPath ? GetDatabaseEngine().ToString() : "Generic";
            return EmbeddedResource.GetAsString(name, $"Sql.{path}.{nestedPath}");
        }

        public DbConnection CreateConnection()
        {
            var engine = GetDatabaseEngine();
            var connectionString = engine == DatabaseEngine.SqlServer
                ? _configurationService.GetConnectionString("SqlServerDatabaseConnection")
                : _configurationService.GetConnectionString("PostgresDatabaseConnection");

            return engine == DatabaseEngine.SqlServer
                ? new SqlConnection(connectionString) as DbConnection
                : new NpgsqlConnection(connectionString);
        }

        public DatabaseEngine GetDatabaseEngine()
        {
            string engineConfig = _configurationService.GetSetting<string>("DatabaseSettings:Engine");
            return Enum.TryParse<DatabaseEngine>(engineConfig, ignoreCase: true, out var result)
                ? result
                : DatabaseEngine.Postgres;
        }
    }
}
