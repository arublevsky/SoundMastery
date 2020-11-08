using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Profile
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserEmailStore<User> _userStore;

        public UserProfileService(IUserEmailStore<User> userStore)
        {
            _userStore = userStore;
        }

        public async Task<UserProfile> GetUserProfile(string email, CancellationToken token)
        {
            User user = await GetUser(email, token);
            var profile = new UserProfile
            {
                Email = email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            return profile;
        }

        public async Task SaveUserProfile(UserProfile profile, CancellationToken token)
        {
            User user = await GetUser(profile.Email, token);

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.PhoneNumber = profile.PhoneNumber;

            await _userStore.UpdateAsync(user, token);
        }

        private async Task<User> GetUser(string email, CancellationToken token)
        {
            User? user = await _userStore.FindByEmailAsync(email.ToUpperInvariant(), token);
            if (user == null)
            {
                throw new InvalidOperationException($"Cannot find a user {email}");
            }
            return user;
        }
    }
}
