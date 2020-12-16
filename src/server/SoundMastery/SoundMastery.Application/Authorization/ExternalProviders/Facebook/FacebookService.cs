using System;
using System.Text.Json;
using System.Threading.Tasks;
using Facebook;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;

namespace SoundMastery.Application.Authorization.ExternalProviders.Facebook
{
    public class FacebookService : IFacebookService
    {
        private readonly ISystemConfigurationService _configurationService;

        public FacebookService(ISystemConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        public async Task<User> GetUserData(string accessToken)
        {
            FacebookClient client = CreateFacebookClient(accessToken);

            await ValidateAccessToken(client);

            var model = await client.GetTaskAsync<MeFacebookModel>($"me?fields=first_name,last_name,email");

            if (model == null || !model.IsValid())
            {
                throw new InvalidOperationException("Cannot find a Facebook user by provided access token.");
            }

            return new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
            };
        }

        private static FacebookClient CreateFacebookClient(string accessToken)
        {
            var client = new FacebookClient(accessToken);

            client.SetJsonSerializers(
                value => JsonSerializer.Serialize(value),
                (value, type) => type != null ? JsonSerializer.Deserialize(value, type) : null);

            return client;
        }

        private async Task ValidateAccessToken(FacebookClient client)
        {
            var appId = _configurationService.GetSetting<string>("Authentication:Facebook:AppId");
            var appSecret = _configurationService.GetSetting<string>("Authentication:Facebook:AppSecret");

            var result = await client.GetTaskAsync<InspectTokenResponse>(
                $"debug_token?input_token={client.AccessToken}&access_token={appId}|{appSecret}");

            if (result?.Data == null || result.Data.IsValid == false)
            {
                throw new InvalidOperationException("Invalid Facebook token provided.");
            }
        }
    }
}
