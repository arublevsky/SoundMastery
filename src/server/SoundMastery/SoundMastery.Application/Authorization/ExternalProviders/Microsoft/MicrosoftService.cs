using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using User = SoundMastery.Domain.Identity.User;

namespace SoundMastery.Application.Authorization.ExternalProviders.Microsoft
{
    public class MicrosoftService : IMicrosoftService
    {
        public async Task<User> GetUserData(string accessToken)
        {
            var provider = new DelegateAuthenticationProvider(request =>
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return Task.CompletedTask;
            });

            var client = new GraphServiceClient(provider);
            var user = await client.Me.Request().GetAsync();

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
}
