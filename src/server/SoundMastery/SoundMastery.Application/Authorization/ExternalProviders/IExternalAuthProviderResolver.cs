namespace SoundMastery.Application.Authorization.ExternalProviders
{
    public interface IExternalAuthProviderResolver
    {
        IExternalAuthProviderService Resolve(ExternalAuthProviderType type);
    }
}
