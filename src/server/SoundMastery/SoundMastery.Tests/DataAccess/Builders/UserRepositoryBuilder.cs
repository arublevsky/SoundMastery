using System;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Services;

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

            return new UserRepository(() => engine, _configuration);
        }
    }
}
