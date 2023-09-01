namespace SoundMastery.Application.Authorization.ExternalProviders;

public class ExternalLoginModel
{
    // keep init; for property bindings
    public ExternalAuthProviderType? Type { get; init; } = null;

    public string AccessToken { get; init; } = string.Empty;
}