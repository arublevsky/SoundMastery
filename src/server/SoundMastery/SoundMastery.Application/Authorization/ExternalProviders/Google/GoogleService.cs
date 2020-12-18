using System.Threading.Tasks;
using Google.Apis.Auth;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;

namespace SoundMastery.Application.Authorization.ExternalProviders.Google
{
    public class GoogleService : IGoogleService
    {
        private readonly ISystemConfigurationService _configurationService;

        public GoogleService(ISystemConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<User> GetUserData(string token)
        {
            var payload = await GetSignaturePayload(token);
            return new User
            {
                UserName = payload.Email,
                Email = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                EmailConfirmed = payload.EmailVerified,
            };
        }

        private async Task<GoogleJsonWebSignature.Payload> GetSignaturePayload(string accessToken)
        {
            var appId = _configurationService.GetSetting<string>("Authentication:Google:ClientId");

            return await GoogleJsonWebSignature.ValidateAsync(
                accessToken,
                new GoogleJsonWebSignature.ValidationSettings { Audience = new [] { appId } });
        }
    }
}
