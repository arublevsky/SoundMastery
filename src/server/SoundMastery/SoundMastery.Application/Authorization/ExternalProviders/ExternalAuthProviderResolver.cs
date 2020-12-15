using System;
using SoundMastery.Application.Authorization.ExternalProviders.Facebook;
using SoundMastery.Application.Authorization.ExternalProviders.Google;

namespace SoundMastery.Application.Authorization.ExternalProviders
{
    public class ExternalAuthProviderResolver : IExternalAuthProviderResolver
    {
        private readonly IFacebookService _facebookService;
        private readonly IGoogleService _googleService;

        public ExternalAuthProviderResolver(IFacebookService facebookService, IGoogleService googleService)
        {
            _facebookService = facebookService;
            _googleService = googleService;
        }

        public IExternalAuthProviderService Resolve(ExternalAuthProviderType type) =>
            type switch
            {
                ExternalAuthProviderType.Facebook => _facebookService,
                ExternalAuthProviderType.Google => _googleService,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown external provider type.")
            };
    }
}
