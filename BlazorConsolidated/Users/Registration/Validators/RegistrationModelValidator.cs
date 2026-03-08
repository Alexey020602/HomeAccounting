using ClientServerShared.Model;
using FluentValidation;

namespace BlazorConsolidated.Users.Registration.Validators;

internal class RegistrationModelValidator: AbstractValidator<RegistrationModel>
{
    public RegistrationModelValidator(/*IAuthorizationApi authorizationApi*/)
    {
        RuleFor(model => model.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Необходимо ввести логин")
            .Matches("^[a-zA-Z0-9-]+$")
            .WithMessage("Логин пользователя может содержать только буквы, цифры и символ дефис (-)")
            // .MustAsync(async (login, _) =>
            //     !await authorizationApi.CheckLoginExist(login)
            // )
            // .WithMessage("Логин {PropertyValue} уже существует")
            ;
        RuleFor(model => model.FullName)
            .NotEmpty()
            .WithMessage("Необходимо ввести имя пользователя")
            .Matches("^[a-zA-Zа-яА-Я0-9 -]+$")
            .WithMessage("Имя пользователя может содержать только буквы (латиница/кириллица), цифры, пробел и дефис");

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