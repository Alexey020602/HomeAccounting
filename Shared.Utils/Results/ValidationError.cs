using FluentValidation.Results;
using MaybeResults;

namespace Shared.Utils.Results;

[None]
public sealed partial record ValidationError<T>
{
    public ValidationError(ValidationResult validationResult)
    {
        if (validationResult.IsValid)
        {
            throw new InvalidOperationException("ValidationErrorResult cannot be created from a successful validation");
        }

        Message = typeof(T).Name;
        
        Details = validationResult
            .Errors
            .Select(e => new NoneDetail(e.PropertyName, e.ErrorMessage))
            .ToList();
    }
}