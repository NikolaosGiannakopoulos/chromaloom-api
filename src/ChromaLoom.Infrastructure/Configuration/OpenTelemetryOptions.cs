using FluentValidation;

namespace ChromaLoom.Infrastructure.Configuration;

public sealed class OpenTelemetryOptions
{
    public const string SectionName = "OpenTelemetry";

    public string OtlpEndpoint { get; init; } = string.Empty;
}

public sealed class OpenTelemetryOptionsValidator : AbstractValidator<OpenTelemetryOptions>
{
    public OpenTelemetryOptionsValidator()
    {
        RuleFor(options => options.OtlpEndpoint)
            .NotEmpty()
            .Must(endpoint => Uri.TryCreate(endpoint, UriKind.Absolute, out _))
            .WithMessage("OtlpEndpoint must be an absolute URI.");
    }
}
