using System.Data.Common;
using SoundMastery.DataAccess.Common;

namespace SoundMastery.DataAccess.Services;

public interface IDatabaseConnectionService
{
    DbConnection CreateConnection();

    (DatabaseEngine, string) GetConnectionParameters();

    string GetSqlPath(string name, string nestedPath, bool useEngineSpecificPath = false);
}