using FluentValidation;
using SoundMastery.Application.Models;

namespace SoundMastery.Application.Validation;

public class UserProfileValidator : AbstractValidator<UserModel>
{
    public UserProfileValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}