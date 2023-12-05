using Microsoft.EntityFrameworkCore;
using SoundMastery.DataAccess.Contexts;
using SoundMastery.Domain.Core;
using SoundMastery.Domain.Services;

namespace SoundMastery.Tests.DataAccess.Contexts;

public class TestContext : SoundMasteryContext
{
    public TestContext(ISystemConfigurationService service) : base(service)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>().Navigation(e => e.Product).AutoInclude();
        modelBuilder.Entity<Course>().Navigation(e => e.Teacher).AutoInclude();
    }
}