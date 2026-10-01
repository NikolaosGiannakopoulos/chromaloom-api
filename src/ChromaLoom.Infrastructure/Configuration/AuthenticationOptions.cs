using FluentValidation;

namespace ChromaLoom.Infrastructure.Configuration;

public sealed class AuthenticationOptions
{
    public const string SectionName = "Authentication";

    public string Authority { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;
}

public sealed class AuthenticationOptionsValidator : AbstractValidator<AuthenticationOptions>
{
    public AuthenticationOptionsValidator()
    {
        RuleFor(options => options.Authority)
            .NotEmpty()
            .Must(authority => Uri.TryCreate(authority, UriKind.Absolute, out _))
            .WithMessage("Authority must be an absolute URI (Keycloak realm URL).");

        RuleFor(options => options.Audience)
            .NotEmpty();
    }
}
