using FluentValidation;

namespace Authorization.UI.Registration.Validators;

sealed class PasswordConfirmationRegistrationValidator : AbstractValidator<string>
{
    public PasswordConfirmationRegistrationValidator(string password) => RuleFor(p => p)
        .NotEmpty()
        .WithMessage("Необходимо подтвердить пароль")
        .Equal(password)
        .WithMessage("Пароли не совпадают");
}