using System;
using Microsoft.Extensions.Configuration;
using SoundMastery.DataAccess.Services.Common;
using SoundMastery.Domain;
using SoundMastery.Domain.Services;
using SoundMastery.Tests.DataAccess.Contexts;

namespace SoundMastery.Tests.DataAccess.Builders;

public class GenericRepositoryBuilder<TEntity>
    where TEntity : BaseEntity
{
    private IConfiguration _configuration;

    public GenericRepositoryBuilder<TEntity> With(IConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    public IGenericRepository<TEntity> Build()
    {
        if (_configuration == null)
        {
            throw new InvalidOperationException("Configuration is not specified");
        }

        var manager = new DatabaseManagerBuilder().With(_configuration).Build();
        manager.MigrateUp().GetAwaiter().GetResult();
        var configurationService = new SystemConfigurationService(_configuration);
        return new GenericRepository<TEntity>(new TestContext(configurationService));
    }
}