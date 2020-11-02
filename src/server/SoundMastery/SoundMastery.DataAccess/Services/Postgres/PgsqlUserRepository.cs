using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SoundMastery.DataAccess.Common;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Postgres
{
    public class PgsqlUserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private const string SqlPath = "Sql.Postgres.Users";

        public PgsqlUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgresDatabaseConnection");
        }

        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var sql = EmbeddedResource.GetAsString("CreateUser.sql", SqlPath);
            await connection.QueryAsync(sql, user);
        }

        public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var sql = EmbeddedResource.GetAsString("FindUserByName.sql", SqlPath);
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { normalizedUserName });
        }

        public async Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var sql = EmbeddedResource.GetAsString("FindUserByEmail.sql", SqlPath);
            return await connection.QuerySingleOrDefaultAsync<User>(sql, new { normalizedEmail });
        }
    }
}
