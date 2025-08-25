using FluentValidation;
using FluentValidation.Results;
using Microsoft.CodeAnalysis;

namespace Shared.Blazor.Validation;

public static class AbstractValidationExtensions
{
    public static Func<object, string, Task<IEnumerable<string>>> ValidateValueAsyncDelegate<T>(
        this AbstractValidator<T> validator) => async (model, propertyName)
        => (await validator.ValidateValueAsync(model, propertyName)).GetErrors();

    public static Func<object, string, IEnumerable<string>> ValidateValueDelegate<T>(
        this AbstractValidator<T> validator) => (model, propertyName)
        => validator.ValidateValue(model, propertyName).GetErrors();

    private static Task<ValidationResult> ValidateValueAsync<T>(
        this AbstractValidator<T> validator,
        object model,
        string propertyName) => validator.ValidateAsync(CreateValidationContext<T>(model, propertyName));

    private static ValidationResult ValidateValue<T>(
        this AbstractValidator<T> validator,
        object model,
        string propertyName) => validator.Validate(CreateValidationContext<T>(model, propertyName));

    private static ValidationContext<T> CreateValidationContext<T>(object model, string propertyName) =>
        ValidationContext<T>.CreateWithOptions(
            (T)model,
            x => x.IncludeProperties(propertyName)
        );
}

public static class FluentValidationResultExtensions
{
    public static IEnumerable<string> GetErrors(this ValidationResult validationResult) =>
        validationResult.IsValid ? [] : validationResult.Errors.Select(x => x.ErrorMessage);
}