using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly IDatabaseConnectionService _connectionService;

        public UserRepository(IDatabaseConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public Task CreateAsync(User user)
        {
            return ExecuteUserQuery("CreateUser.sql", user);
        }

        public Task<User?> FindByNameAsync(string userName)
        {
            return FindUserQuery("FindUserByName.sql", new
            {
                NormalizedUserName = userName.ToUpperInvariant()
            });
        }

        public Task<User?> FindByEmailAsync(string email)
        {
            return FindUserQuery("FindUserByEmail.sql", new
            {
                NormalizedEmail = email.ToUpperInvariant()
            });
        }

        public Task UpdateAsync(User user)
        {
            return ExecuteUserQuery("UpdateUser.sql", user);
        }

        public Task AssignRefreshToken(string token, User user)
        {
            return ExecuteUserQuery("InsertRefreshToken.sql", new
            {
                UserId = user.Id,
                CreatedAtUtc = DateTime.UtcNow,
                Token = token,
            });
        }

        public Task ClearRefreshToken(User user)
        {
            return ExecuteUserQuery("ClearRefreshToken.sql", new { UserId = user.Id });
        }

        private async Task ExecuteUserQuery(string sqlName, object user)
        {
            await using var connection = _connectionService.CreateConnection();
            await connection.OpenAsync();
            await connection.QueryAsync(GetSql(sqlName), user);
        }

        private async Task<User?> FindUserQuery(string sqlName, object options)
        {
            await using var connection = _connectionService.CreateConnection();
            await connection.OpenAsync();

            var user = await connection.QuerySingleOrDefaultAsync<User>(GetSql(sqlName), options);
            if (user != null)
            {
                using var multi = await connection.QueryMultipleAsync(
                    GetSql("UserCollections.sql"),
                    new { user.Id });

                user.Roles = multi.Read<Role>().ToList();
                user.RefreshTokens = multi.Read<RefreshToken>().ToList();
            }

            return user;
        }

        private string GetSql(string name) => _connectionService.GetSqlPath(name, "Users");
    }
}
