using System.Data.Common;
using SoundMastery.DataAccess.Common;

namespace SoundMastery.DataAccess.Services.Common
{
    public interface IDatabaseConnectionService
    {
        DbConnection CreateConnection();

        DatabaseEngine GetDatabaseEngine();

        string GetSqlPath(string name, string nestedPath, bool useEngineSpecificPath = false);
    }
}
