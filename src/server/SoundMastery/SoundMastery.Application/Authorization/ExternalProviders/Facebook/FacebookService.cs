using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;

namespace SoundMastery.Application.Authorization.ExternalProviders.Facebook
{
    public class FacebookService : IFacebookService
    {
        private readonly HttpClient _httpClient;
        private readonly ISystemConfigurationService _configurationService;

        public FacebookService(HttpClient httpClient, ISystemConfigurationService configurationService)
        {
            _httpClient = httpClient;
            _configurationService = configurationService;
        }

        public async Task<User> GetUserData(string accessToken)
        {
            await ValidateAccessToken(accessToken);

            var model = await CallFacebookApi<MeFacebookModel>(
                $"me?fields=first_name,last_name,email&access_token={accessToken}");

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

        private async Task ValidateAccessToken(string accessToken)
        {
            var appId = _configurationService.GetSetting<string>("Authentication:Facebook:AppId");
            var appSecret = _configurationService.GetSetting<string>("Authentication:Facebook:AppSecret");

            var result = await CallFacebookApi<InspectTokenResponse>(
                $"debug_token?input_token={accessToken}&access_token={appId}|{appSecret}");

            if (result?.Data == null || result.Data.IsValid == false)
            {
                throw new InvalidOperationException("Invalid Facebook token provided.");
            }
        }


        private async Task<T?> CallFacebookApi<T>(string query)
            where T: class
        {
            var response = await _httpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(result);
        }
    }
}
