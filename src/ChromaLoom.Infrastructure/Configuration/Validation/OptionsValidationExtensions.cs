using FluentValidation;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChromaLoom.Infrastructure.Configuration.Validation;

public static class OptionsValidationExtensions
{
    public static OptionsBuilder<TOptions> AddValidatedOptions<TOptions, TValidator>(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName)
        where TOptions : class
        where TValidator : class, IValidator<TOptions>
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentException.ThrowIfNullOrWhiteSpace(sectionName);

        services.TryAddSingleton<IValidator<TOptions>, TValidator>();
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IValidateOptions<TOptions>, FluentValidateOptions<TOptions>>());

        return services
            .AddOptions<TOptions>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateOnStart();
    }
}
