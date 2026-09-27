using Microsoft.EntityFrameworkCore;

namespace ChromaLoom.Infrastructure.Persistence.QueryFilters;

internal static class QueryFilterExtensions
{
    extension(ModelBuilder modelBuilder)
    {
        internal ModelBuilder ApplyQueryFilters(params IQueryFilter[] filters)
        {
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
}
