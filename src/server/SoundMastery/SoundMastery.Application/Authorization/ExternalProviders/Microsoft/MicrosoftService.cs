using System;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Graph;
using User = SoundMastery.Domain.Identity.User;

namespace SoundMastery.Application.Authorization.ExternalProviders.Microsoft;

public class MicrosoftService : IMicrosoftService
{
    public async Task<User> GetUserData(string token)
    {
        var client = new GraphServiceClient(DelegatedTokenCredential.Create((_, _) =>
            new AccessToken(token, DateTimeOffset.Now.AddHours(1))));

        var user = await client.Me.GetAsync();
        if (user == null)
        {
            return null;
        }

        return new User
        {
            UserName = user.UserPrincipalName,
            Email = user.UserPrincipalName,
            FirstName = user.GivenName,
            LastName = user.Surname,
            EmailConfirmed = true
        };
    }
}