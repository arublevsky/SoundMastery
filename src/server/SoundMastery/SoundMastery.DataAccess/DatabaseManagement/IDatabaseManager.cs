using System.Threading.Tasks;

namespace SoundMastery.DataAccess.DatabaseManagement
{
    public interface IDatabaseManager
    {
        Task EnsureDatabaseCreated();

        Task CreateDatabase();

        Task CheckConnection();

        Task Drop();
    }
}
