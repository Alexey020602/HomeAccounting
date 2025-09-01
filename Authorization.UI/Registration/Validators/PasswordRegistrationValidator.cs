using FluentValidation;
using Shared.Utils.Model;

namespace Authorization.UI.Registration.Validators;

sealed class PasswordRegistrationValidator : AbstractValidator<string>
{
    public PasswordRegistrationValidator()
    {
        RuleFor(p => p)
            .Cascade(CascadeMode.Continue)
            .Password();
    }
}