using System.Threading.Tasks;

namespace SoundMastery.Migration.Services
{
    public interface IMigrationService
    {
        Task MigrateUp();

        Task ApplySeeds();
    }
}
