using System;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Common;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.DataAccess.Services.Postgres;
using SoundMastery.DataAccess.Services.SqlServer;

namespace SoundMastery.Tests.DataAccess.Builders
{
    public class DatabaseManagerBuilder
    {
        private IConfiguration? _configuration;

        public DatabaseManagerBuilder With(IConfiguration configuration)
        {
            _configuration = configuration;
            return this;
        }

        public IDatabaseManager Build(DatabaseEngine engine)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("Configuration is not specified");
            }

            return engine switch
            {
                DatabaseEngine.Postgres => new PgsqlDatabaseManager(_configuration),
                DatabaseEngine.SqlServer => new SqlServerDatabaseManager(_configuration),
                _ => throw new ArgumentOutOfRangeException(nameof(engine), engine, $"Unknown engine {engine}")
            };
        }
    }
}
