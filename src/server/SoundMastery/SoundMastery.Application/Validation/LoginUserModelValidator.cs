using FluentValidation;
using SoundMastery.Application.Authorization;

namespace SoundMastery.Application.Validation
{
    public class LoginUserModelValidator : AbstractValidator<LoginUserModel>
    {
        public LoginUserModelValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
