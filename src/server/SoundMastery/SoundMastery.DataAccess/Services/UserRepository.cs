using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SoundMastery.DataAccess.Common;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseEngineAccessor _accessor;
        private readonly IConfiguration _configuration;

        public UserRepository(DatabaseEngineAccessor accessor, IConfiguration configuration)
        {
            _accessor = accessor;
            _configuration = configuration;
        }

        public Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            return ExecuteUserQuery("CreateUser.sql", user, cancellationToken);
        }

        public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return ExecuteSingleQuery("FindUserByName.sql", new { normalizedUserName }, cancellationToken);
        }

        public Task<User?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return ExecuteSingleQuery("FindUserByEmail.sql", new { normalizedEmail }, cancellationToken);
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            return ExecuteUserQuery("UpdateUser.sql", user, cancellationToken);
        }

        private async Task ExecuteUserQuery(string sqlName, User user, CancellationToken cancellationToken)
        {
            await using var connection = CreateConnection();
            await connection.OpenAsync(cancellationToken);
            await connection.QueryAsync(GetSql(sqlName), user);
        }

        private async Task<User?> ExecuteSingleQuery(string sqlName, object options, CancellationToken cancellationToken)
        {
            await using var connection = CreateConnection();
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<User>(GetSql(sqlName), options);
        }

        private DbConnection CreateConnection()
        {
            var connectionString = _accessor() == DatabaseEngine.SqlServer
                ? _configuration.GetConnectionString("SqlServerDatabaseConnection")
                : _configuration.GetConnectionString("PostgresDatabaseConnection");

            return _accessor() == DatabaseEngine.SqlServer
                ? new SqlConnection(connectionString) as DbConnection
                : new NpgsqlConnection(connectionString);
        }

        private string GetSql(string name) => EmbeddedResource.GetAsString(name, $"Sql.{_accessor()}.Users");
    }
}
