using Microsoft.EntityFrameworkCore;

namespace ChromaLoom.Infrastructure.Persistence.QueryFilters;

internal static class QueryFilterExtensions
{
    public static ModelBuilder ApplyQueryFilters(
        this ModelBuilder modelBuilder,
        params IQueryFilter[] filters)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        ArgumentNullException.ThrowIfNull(filters);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned() || entityType.IsKeyless || entityType.BaseType is not null)
            {
                continue;
            }

            foreach (var filter in filters)
            {
                if (!filter.AppliesTo(entityType.ClrType))
                {
                    continue;
                }

                modelBuilder
                    .Entity(entityType.ClrType)
                    .HasQueryFilter(filter.Name, filter.GetFilter(entityType.ClrType));
            }
        }

        return modelBuilder;
    }
}
