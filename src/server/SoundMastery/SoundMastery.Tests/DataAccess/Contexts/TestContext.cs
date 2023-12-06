using Microsoft.EntityFrameworkCore;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain.Services;

namespace SoundMastery.Tests.DataAccess.Contexts;

public class TestContext : SoundMasteryContext
{
    public TestContext(ISystemConfigurationService service) : base(service)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // to add custom configs
    }
}