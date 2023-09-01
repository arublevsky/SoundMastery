using SoundMastery.Domain.Services;

namespace SoundMastery.DataAccess.Services;

public class DatabaseConnectionService : IDatabaseConnectionService
{
    private readonly ISystemConfigurationService _configurationService;

    public DatabaseConnectionService(ISystemConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }

    public string GetConnectionString() => _configurationService.GetConnectionString("SqlServerDatabaseConnection");

}