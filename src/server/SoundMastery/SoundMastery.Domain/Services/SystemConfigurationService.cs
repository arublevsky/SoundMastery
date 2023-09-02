using System;
using Microsoft.Extensions.Configuration;

namespace SoundMastery.Domain.Services;

public class SystemConfigurationService : ISystemConfigurationService
{
    private readonly IConfiguration _configuration;

    public SystemConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public T GetSetting<T>(string key)
    {
        var value = _configuration[key];
        AssertSettingExists(key, value);
        return (T)Convert.ChangeType(value, typeof(T));
    }

    public string GetConnectionString(string name)
    {
        var value = _configuration.GetConnectionString(name);
        AssertSettingExists(name, value);
        return value;
    }

    private static void AssertSettingExists(string key, string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"The system configuration setting was not found for {key}");
        }
    }
}