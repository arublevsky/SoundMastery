using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Common;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.SqlServer
{
    public class SqlServerUserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private const string SqlPath = "Sql.SqlServer.Users";

        public SqlServerUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServerDatabaseConnection");
        }

        public Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            return ExecuteUserQuery("CreateUser.sql", user, cancellationToken);
        }

        public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return ExecuteUserSingleQuery("FindUserByName.sql", new { normalizedUserName }, cancellationToken);
        }

        public Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return ExecuteUserSingleQuery("FindUserByEmail.sql", new { normalizedEmail }, cancellationToken);
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return ExecuteUserQuery("UpdateUser.sql", user, cancellationToken);
        }

        private async Task ExecuteUserQuery(string sqlName, User user, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var sql = EmbeddedResource.GetAsString(sqlName, SqlPath);
            await connection.QueryAsync(sql, user);
        }

        private async Task<User?> ExecuteUserSingleQuery(string sqlName, object options, CancellationToken cancellationToken)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var sql = EmbeddedResource.GetAsString(sqlName, SqlPath);
            return await connection.QuerySingleOrDefaultAsync<User>(sql, options);
        }
    }
}
