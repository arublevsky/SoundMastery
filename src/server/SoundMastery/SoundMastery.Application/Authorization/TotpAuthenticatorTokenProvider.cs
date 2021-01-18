using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SoundMastery.Domain.Identity;

namespace SoundMastery.Application.Authorization
{
    public class TotpAuthenticatorTokenProvider : IUserTwoFactorTokenProvider<User>
    {
        private const int PreviousCodesNumber = 2;
        private const int FutureCodesNumber = 2;

        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(user.MfaAuthenticationKey != null);
        }

        public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
        {
            throw new NotImplementedException("We don't generate tokens");
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
        {
            var otp = new Totp(Base32Encoding.ToBytes(user.MfaAuthenticationKey));
            var valid = otp.VerifyTotp(token, out _, new VerificationWindow(PreviousCodesNumber, FutureCodesNumber));

            return Task.FromResult(valid);
        }
    }
}
