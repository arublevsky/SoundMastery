using System;
using SoundMastery.Application.Authorization.ExternalProviders.Facebook;
using SoundMastery.Application.Authorization.ExternalProviders.Google;
using SoundMastery.Application.Authorization.ExternalProviders.Microsoft;

namespace SoundMastery.Application.Authorization.ExternalProviders
{
    public class ExternalAuthProviderResolver : IExternalAuthProviderResolver
    {
        private readonly IFacebookService _facebookService;
        private readonly IGoogleService _googleService;
        private readonly IMicrosoftService _microsoftService;

        public ExternalAuthProviderResolver(
            IFacebookService facebookService,
            IGoogleService googleService,
            IMicrosoftService microsoftService)
        {
            _facebookService = facebookService;
            _googleService = googleService;
            _microsoftService = microsoftService;
        }

        public IExternalAuthProviderService Resolve(ExternalAuthProviderType type) =>
            type switch
            {
                ExternalAuthProviderType.Facebook => _facebookService,
                ExternalAuthProviderType.Google => _googleService,
                ExternalAuthProviderType.Microsoft => _microsoftService,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown external provider type.")
            };
    }
}
