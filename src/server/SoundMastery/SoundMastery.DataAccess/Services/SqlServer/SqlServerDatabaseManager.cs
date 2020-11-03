using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Common;

namespace SoundMastery.DataAccess.Services.SqlServer
{
    public class SqlServerDatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;
        private const string SqlPath = "Sql.SqlServer.DatabaseManagement";

        public SqlServerDatabaseManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServerServerConnection");
        }

        public async Task EnsureDatabaseCreated()
        {
            if (!await DatabaseExists())
            {
                await CreateDatabase();
            }
        }

        public async Task<bool> DatabaseExists()
        {
            await using var conn = new SqlConnection(_connectionString);
            string sql = EmbeddedResource.GetAsString("CheckDatabaseExists.sql", SqlPath);

            await using var command = new SqlCommand(sql, conn);
            try
            {
                await conn.OpenAsync();
                object? result = await command.ExecuteScalarAsync();
                await conn.CloseAsync();
                return result != null && result.Equals("soundmastery");
            }
            catch (Exception e)
            {
                // TODO: add proper logging https://github.com/arublevsky/SoundMastery/issues/14
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task CheckConnection()
        {
            try
            {
                await using var conn = new SqlConnection(_connectionString);
                await using SqlCommand command = new SqlCommand("SELECT 1", conn);
                await conn.OpenAsync();
                await command.ExecuteScalarAsync();
                await conn.CloseAsync();
            }
            catch
            {
                throw new Exception("Database is not available...");
            }
        }

        public async Task Drop()
        {
            var sql = EmbeddedResource.GetAsString("DropDatabase.sql", SqlPath);

            await using var conn = new SqlConnection(_connectionString);
            await using SqlCommand command = new SqlCommand(sql, conn);
            await conn.OpenAsync();
            await command.ExecuteScalarAsync();
            await conn.CloseAsync();
        }

        private async Task CreateDatabase()
        {
            await using var conn = new SqlConnection(_connectionString);
            string sql = EmbeddedResource.GetAsString("CreateDatabase.sql", SqlPath);

            await using SqlCommand command = new SqlCommand(sql, conn);
            try
            {
                await conn.OpenAsync();
                await command.ExecuteScalarAsync();
                await conn.CloseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
