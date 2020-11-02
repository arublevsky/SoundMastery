using System;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Services;
using SoundMastery.DataAccess.Services.Postgres;
using SoundMastery.DataAccess.Services.SqlServer;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class UserRepositoryBuilder
    {
        private IConfiguration _configuration;

        public UserRepositoryBuilder With(IConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public IUserRepository Build(DatabaseEngine engine)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration is not specified");
            }

            return engine switch
            {
                DatabaseEngine.Postgres => new PgsqlUserRepository(_configuration),
                DatabaseEngine.SqlServer => new SqlServerUserRepository(_configuration),
                _ => throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}")
            };
        }
    }
}
