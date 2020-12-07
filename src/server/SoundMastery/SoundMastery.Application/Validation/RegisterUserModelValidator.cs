using FluentValidation;
using SoundMastery.Application.Authorization;

namespace SoundMastery.Application.Validation
{
    public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
    {
        public RegisterUserModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
