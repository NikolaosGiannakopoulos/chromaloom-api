using Microsoft.EntityFrameworkCore;
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

        if (GetType().Namespace is { } currentNamespace)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                GetType().Assembly,
                type => type.Namespace?.StartsWith(currentNamespace, StringComparison.Ordinal) == true);
        }

        modelBuilder.ApplyQueryFilters(QueryFilters);
    }
}
