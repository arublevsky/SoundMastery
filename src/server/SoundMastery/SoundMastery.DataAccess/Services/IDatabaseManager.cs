using System.Threading.Tasks;

namespace SoundMastery.DataAccess.Services;

public interface IDatabaseManager
{
    Task CheckConnection();

    Task Drop();

    Task MigrateUp();
}