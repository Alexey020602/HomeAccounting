using FluentValidation;
using FluentValidation.Validators;

namespace Authorization.UI.Registration.Validators;

// sealed class LoginRegistrationValidator: NoopPropertyValidator<>
//
//     public LoginRegistrationValidator(IAuthorizationApi authorizationApi)
//     {
//         RuleFor(p => p)
//             
//             .Cascade(CascadeMode.Stop)
//             .NotEmpty()
//             .WithMessage("Необходимо ввести логин")
//             .MustAsync(async (login, _) =>
//             {
//                 if (string.IsNullOrEmpty(login))
//                 {
//                     return false;
//                 }
//                 return !await authorizationApi.CheckLoginExist(login);
//             })
//             .WithMessage("Логин {PropertyValue} уже существует")
//             ;
//     }
// }