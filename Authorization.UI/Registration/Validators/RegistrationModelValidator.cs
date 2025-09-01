using FluentValidation;
using Shared.Utils.Model;

namespace Authorization.UI.Registration.Validators;

internal class RegistrationModelValidator: AbstractValidator<RegistrationModel>
{
    public RegistrationModelValidator(IAuthorizationApi authorizationApi)
    {
        RuleFor(model => model.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Необходимо ввести логин")
            .MustAsync(async (login, _) =>
                !await authorizationApi.CheckLoginExist(login)
            )
            .WithMessage("Логин {PropertyValue} уже существует")
            ;
        RuleFor(model => model.UserName)
            .NotEmpty()
            .WithMessage("Необходимо ввести имя пользователя");

        RuleFor(model => model.Password)
            .Cascade(CascadeMode.Continue)
            .Password();

        RuleFor(model => model.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Необходимо подтвердить пароль")
            .Equal(model => model.Password)
            .WithMessage("Пароли не совпадают");
    }
}