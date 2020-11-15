namespace SoundMastery.Domain.Services
{
    public interface ISystemConfigurationService
    {
        T GetSetting<T>(string key);

        string GetConnectionString(string name);
    }
}
