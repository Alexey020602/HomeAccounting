using FluentValidation;

namespace Authorization.UI.Registration.Validators;

sealed class UserNameRegistrationValidator : AbstractValidator<string>
{
    public UserNameRegistrationValidator() => RuleFor(p => p)
        .NotEmpty()
        .WithMessage("Необходимо ввести имя пользователя");
}