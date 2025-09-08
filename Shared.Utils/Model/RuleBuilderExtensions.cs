using FluentValidation;

namespace Shared.Utils.Model;

public static class RuleBuilderExtensions
{
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty()
        .WithMessage("Необходимо ввести пароль")
        .MinimumLength(6)
        .WithMessage("Пароль должен содержать минимум 6 символов")
        .Matches("[A-ZА-Я]")
        .WithMessage("Пароль должен содержать одну заглавную латинскую букву")
        .Matches("[a-zа-я]")
        .WithMessage("Пароль должен содержать одну строчную букву")
        .Matches("[0-9]")
        .WithMessage("Пароль должен содержать одну цифру")
        .Matches("[^a-zA-Z0-9]")
        .WithMessage("Пароль должен содержать один символ не букву и не цифру")
        .Matches("^[a-zA-Zа-яА-Я0-9!@#$%^&*()-_=+]")
        .WithMessage("Пароль дожен содержать только буквы, цифры и служебные символы (!@#$%^&*()-_=+)");
}