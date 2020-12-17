using System;
using SoundMastery.Application.Authorization.ExternalProviders.Facebook;
using SoundMastery.Application.Authorization.ExternalProviders.Google;
using SoundMastery.Application.Authorization.ExternalProviders.Microsoft;
using SoundMastery.Application.Authorization.ExternalProviders.Twitter;

namespace SoundMastery.Application.Authorization.ExternalProviders
{
    public class ExternalAuthProviderResolver : IExternalAuthProviderResolver
    {
        private readonly IFacebookService _facebookService;
        private readonly IGoogleService _googleService;
        private readonly IMicrosoftService _microsoftService;
        private readonly ITwitterService _twitterService;

        public ExternalAuthProviderResolver(
            IFacebookService facebookService,
            IGoogleService googleService,
            IMicrosoftService microsoftService,
            ITwitterService twitterService)
        {
            _facebookService = facebookService;
            _googleService = googleService;
            _microsoftService = microsoftService;
            _twitterService = twitterService;
        }

        public IExternalAuthProviderService Resolve(ExternalAuthProviderType type) =>
            type switch
            {
                ExternalAuthProviderType.Facebook => _facebookService,
                ExternalAuthProviderType.Google => _googleService,
                ExternalAuthProviderType.Microsoft => _microsoftService,
                ExternalAuthProviderType.Twitter => _twitterService,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown external provider type.")
            };
    }
}
