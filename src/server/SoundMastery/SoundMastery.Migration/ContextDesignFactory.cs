using Microsoft.EntityFrameworkCore.Design;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain.Services;

namespace SoundMastery.Migration;

public class SoundMasteryContextFactory : IDesignTimeDbContextFactory<SoundMasteryContext>
{
    public SoundMasteryContext CreateDbContext(string[] args)
    {
        var config = ConfigurationFactory.Create(args);
        return new SoundMasteryContext(new SystemConfigurationService(config));
    }
}
