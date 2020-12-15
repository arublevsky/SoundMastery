using Microsoft.AspNetCore.Http;
using Moq;
using SoundMastery.Application.Authorization;
using SoundMastery.Application.Authorization.ExternalProviders;
using SoundMastery.Application.Authorization.ExternalProviders.Facebook;
using SoundMastery.Application.Authorization.ExternalProviders.Google;
using SoundMastery.Application.Common;
using SoundMastery.Application.Identity;
using SoundMastery.Application.Profile;
using SoundMastery.Domain.Services;

namespace SoundMastery.Tests.Application.Builders
{
    public class UserAuthorizationServiceBuilder
    {
        private ISystemConfigurationService? _configurationService;
        private IUserService? _userService;
        private IIdentityManager? _identityManager;
        private IHttpContextAccessor? _httpContextAccessor;
        private IDateTimeProvider? _dateTimeProvider;
        private IFacebookService? _facebookService;
        private IGoogleService? _googleService;

        public UserAuthorizationServiceBuilder With(ISystemConfigurationService configuration)
        {
            _configurationService = configuration;
            return this;
        }

        public UserAuthorizationServiceBuilder With(IUserService userService)
        {
            _userService = userService;
            return this;
        }

        public UserAuthorizationServiceBuilder With(IIdentityManager identityManager)
        {
            _identityManager = identityManager;
            return this;
        }

        public UserAuthorizationServiceBuilder With(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            return this;
        }

        public UserAuthorizationServiceBuilder With(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            return this;
        }

        public UserAuthorizationServiceBuilder With(IFacebookService facebookService)
        {
            _facebookService = facebookService;
            return this;
        }

        public UserAuthorizationServiceBuilder With(IGoogleService googleService)
        {
            _googleService = googleService;
            return this;
        }

        public IUserAuthorizationService Build()
        {
            var externalAuthResolver = new ExternalAuthProviderResolver(
                _facebookService ?? new Mock<IFacebookService>().Object,
                _googleService ?? new Mock<IGoogleService>().Object);

            return new UserAuthorizationService(
                _configurationService ?? new Mock<ISystemConfigurationService>().Object,
                _httpContextAccessor ?? new Mock<IHttpContextAccessor>().Object,
                _userService ?? new Mock<IUserService>().Object,
                _identityManager ?? new Mock<IIdentityManager>().Object,
                _dateTimeProvider ?? new Mock<IDateTimeProvider>().Object,
                externalAuthResolver);
        }
    }
}
