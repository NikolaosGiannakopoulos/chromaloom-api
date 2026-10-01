using Microsoft.EntityFrameworkCore;
using ChromaLoom.Kernel.Abstractions.Entities;

namespace ChromaLoom.Infrastructure.Persistence;

internal static class ConcurrencyTokenExtensions
{
    private const string XminPropertyName = "xmin";

    public static ModelBuilder ApplyConcurrencyTokens(this ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.IsOwned()
                || entityType.IsKeyless
                || entityType.BaseType is not null
                || !typeof(IEntity).IsAssignableFrom(entityType.ClrType))
            {
                continue;
            }

            modelBuilder.Entity(entityType.ClrType)
                .Property<uint>(XminPropertyName)
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        }

        return modelBuilder;
    }
}
