using System.Threading.Tasks;
using SoundMastery.Domain.Identity;

namespace SoundMastery.DataAccess.Services
{
    public interface IMigrationService
    {
        Task MigrateUp();

        Task ApplySeeds(User[] users);
    }
}
