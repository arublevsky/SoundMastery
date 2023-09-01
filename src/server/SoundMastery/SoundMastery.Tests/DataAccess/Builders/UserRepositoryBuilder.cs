using System;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Services;
using SoundMastery.DataAccess.Services.Users;
using SoundMastery.Domain.Services;

namespace SoundMastery.Tests.DataAccess.Builders;

public class UserRepositoryBuilder
{
    private IConfiguration _configuration;

    public UserRepositoryBuilder With(IConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    public IUserRepository Build()
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration is not specified");
        }

        var configurationService = new SystemConfigurationService(_configuration);
        var connectionService = new DatabaseConnectionService(configurationService);
        return new UserRepository(connectionService);
    }
}