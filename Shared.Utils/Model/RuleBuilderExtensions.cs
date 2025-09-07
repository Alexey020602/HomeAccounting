using FluentValidation;

namespace Shared.Utils.Model;

public static class RuleBuilderExtensions
{
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder) => ruleBuilder
        .NotEmpty()
        .WithMessage("Необходимо ввести пароль")
        .MinimumLength(6)
        .WithMessage("Пароль должен содержать минимум 6 символов")
        .Matches("[A-Z]")
        .WithMessage("Пароль должен содержать одну заглавную букву")
        .Matches("[a-z]")
        .WithMessage("Пароль должен содержать одну строчную букву")
        .Matches("[0-9]")
        .WithMessage("Пароль должен содержать одну цифру")
        .Matches("[^a-zA-Z0-9]")
        .WithMessage("Пароль должен содержать один символ не букву и не цифру");
}