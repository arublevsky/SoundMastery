using System;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.DataAccess.Services;
using SoundMastery.Domain.Services;

namespace SoundMastery.Tests.DataAccess.Builders;

public class DatabaseManagerBuilder
{
    private IConfiguration _configuration;

    public DatabaseManagerBuilder With(IConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    public IDatabaseManager Build()
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration is not specified");
        }

        var ctx = new SoundMasteryContext(new DatabaseConnectionService(new SystemConfigurationService(_configuration)));
        return new DatabaseManager(ctx);
    }
}