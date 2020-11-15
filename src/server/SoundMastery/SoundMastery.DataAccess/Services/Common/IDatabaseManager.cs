using System.Threading.Tasks;

namespace SoundMastery.DataAccess.Services.Common
{
    public interface IDatabaseManager
    {
        Task EnsureDatabaseCreated();

        Task CheckConnection();

        Task Drop();

        Task<bool> DatabaseExists();
    }
}
