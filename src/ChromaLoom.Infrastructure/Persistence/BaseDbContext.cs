using Microsoft.EntityFrameworkCore;
using ChromaLoom.Kernel.Abstractions.Entities;
using ChromaLoom.Infrastructure.Persistence.QueryFilters;

namespace ChromaLoom.Infrastructure.Persistence;

public abstract class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    public abstract string Schema { get; }

    private static readonly IQueryFilter[] QueryFilters =
    [
        new SoftDeleteQueryFilter()
    ];

    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema);

        var contextNamespace = GetType().Namespace
            ?? throw new InvalidOperationException(
                $"{GetType().Name} must live in a namespace so module configurations can be discovered.");

        modelBuilder.ApplyConfigurationsFromAssembly(
            GetType().Assembly,
            type => type.Namespace is not null
                && (type.Namespace.Equals(contextNamespace, StringComparison.Ordinal)
                    || type.Namespace.StartsWith(contextNamespace + ".", StringComparison.Ordinal)));

        modelBuilder.ApplyQueryFilters(QueryFilters);
        modelBuilder.ApplyConcurrencyTokens();
    }
}
