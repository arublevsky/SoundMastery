using FluentValidation;
using SoundMastery.Application.Authorization.ExternalProviders;

namespace SoundMastery.Application.Validation;

public class ExternalLoginModelValidator : AbstractValidator<ExternalLoginModel>
{
    public ExternalLoginModelValidator()
    {
        RuleFor(x => x.Type).NotNull();
        RuleFor(x => x.AccessToken).NotEmpty();
    }
}