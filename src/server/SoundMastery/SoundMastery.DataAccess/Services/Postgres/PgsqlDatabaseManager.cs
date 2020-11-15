using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SoundMastery.DataAccess.Services.Common;
using EmbeddedResource = SoundMastery.DataAccess.Common.EmbeddedResource;

namespace SoundMastery.DataAccess.Services.Postgres
{
    public class PgsqlDatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;
        private const string SqlPath = "Sql.Postgres.DatabaseManagement";

        public PgsqlDatabaseManager(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresServerConnection");
        }

        public async Task EnsureDatabaseCreated()
        {
            if (!await DatabaseExists())
            {
                await CreateDatabase();
            }
        }

        public async Task CheckConnection()
        {
            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await using var command = new NpgsqlCommand("SELECT 1", conn);
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
            await DropDatabaseConnections();
            await DropDatabase();
        }

        public async Task<bool> DatabaseExists()
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            string sql = EmbeddedResource.GetAsString("CheckDatabaseExists.sql", SqlPath);

            await using var command = new NpgsqlCommand(sql, conn);
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

        private async Task CreateDatabase()
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            string sql = EmbeddedResource.GetAsString("CreateDatabase.sql", SqlPath);

            await using var command = new NpgsqlCommand(sql, conn);
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

        private async Task DropDatabaseConnections()
        {
            var sql = EmbeddedResource.GetAsString("DropConnections.sql", SqlPath);

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();
                await using var command = new NpgsqlCommand(sql, conn);
                await command.ExecuteScalarAsync();
            }
            catch (PostgresException e) when (e.SqlState == "57P01")
            {
                // that's okay, connection lost
            }
        }

        private async Task DropDatabase()
        {
            var sql = EmbeddedResource.GetAsString("DropDatabase.sql", SqlPath);

            await using var conn = new NpgsqlConnection(_connectionString);
            await using var command = new NpgsqlCommand(sql, conn);
            await conn.OpenAsync();
            await command.ExecuteScalarAsync();
            await conn.CloseAsync();
        }
    }
}
