using FluentValidation;
using Microsoft.Extensions.Options;

namespace ChromaLoom.Infrastructure.Configuration.Validation;

internal sealed class FluentValidateOptions<TOptions>(IValidator<TOptions> validator)
    : IValidateOptions<TOptions>
    where TOptions : class
{
    public ValidateOptionsResult Validate(string? name, TOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var result = validator.Validate(options);

        if (result.IsValid)
        {
            return ValidateOptionsResult.Success;
        }

        var failures = result.Errors.Select(failure =>
            $"{typeof(TOptions).Name}.{failure.PropertyName}: {failure.ErrorMessage}");

        return ValidateOptionsResult.Fail(failures);
    }
}
