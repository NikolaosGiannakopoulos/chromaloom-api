using FluentValidation;

namespace ChromaLoom.Infrastructure.Configuration;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";
    public string ConnectionString { get; init; } = string.Empty;
}

internal sealed class DatabaseOptionsValidator : AbstractValidator<DatabaseOptions>
{
    public DatabaseOptionsValidator()
    {
        RuleFor(x => x.ConnectionString)
            .NotEmpty()
            .WithMessage("Database connection string is required.");
    }
}
