using System;
using System.Threading.Tasks;
using SoundMastery.Domain.Identity;
using SoundMastery.Domain.Services;
using Tweetinvi;
using Tweetinvi.Auth;
using Tweetinvi.Parameters;

namespace SoundMastery.Application.Authorization.ExternalProviders.Twitter;

public class TwitterService : ITwitterService
{
    private readonly ISystemConfigurationService _configurationService;
    private readonly IAuthenticationRequestStore _authRequestStore;

    public TwitterService(
        ISystemConfigurationService configurationService,
        IAuthenticationRequestStore authRequestStore)
    {
        _configurationService = configurationService;
        _authRequestStore = authRequestStore;
    }

    public async Task<User> GetUserData(string token)
    {
        var (key, secret, _) = GetConfigurationParameters();
        var client = new TwitterClient(key, secret);

        var parameters = await RequestCredentialsParameters.FromCallbackUrlAsync(token, _authRequestStore);
        var userCredentials = await client.Auth.RequestCredentialsAsync(parameters);
        var userClient = new TwitterClient(userCredentials);
        var user = await userClient.Users.GetAuthenticatedUserAsync();

        return new User
        {
            UserName = user.Email,
            Email = user.Email,
            FirstName = user.Name,
            LastName = user.ScreenName,
            EmailConfirmed = true
        };
    }

    public async Task<string> AcquireRequestToken()
    {
        var (key, secret, clientUrl) = GetConfigurationParameters();
        var client = new TwitterClient(key, secret);

        var authRequestId = Guid.NewGuid().ToString();

        var callbackUrl = _authRequestStore.AppendAuthenticationRequestIdToCallbackUrl(
            $"{clientUrl}/twitter-sign-in-success",
            authRequestId);

        var result = await client.Auth.RequestAuthenticationUrlAsync(callbackUrl);

        await _authRequestStore.AddAuthenticationTokenAsync(authRequestId, result);

        return result.AuthorizationKey;
    }

    private (string key, string secret, string clientUrl) GetConfigurationParameters()
    {
        var key = _configurationService.GetSetting<string>("Authentication:Twitter:ConsumerKey");
        var secret = _configurationService.GetSetting<string>("Authentication:Twitter:ConsumerSecret");
        var clientUrl = _configurationService.GetSetting<string>("ClientUrl");

        return (key, secret, clientUrl);
    }
}