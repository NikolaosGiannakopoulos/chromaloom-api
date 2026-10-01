using Microsoft.EntityFrameworkCore;
using ChromaLoom.Kernel.Abstractions.Entities;
using ChromaLoom.Kernel.Abstractions.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChromaLoom.Infrastructure.Persistence.Interceptors;

internal sealed class SoftDeleteInterceptor(ICurrentUser currentUser) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        Apply(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        Apply(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void Apply(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;
        var userId = currentUser.Id;

        var entries = context.ChangeTracker
            .Entries<ISoftDeletable>()
            .Where(entry => entry.State is EntityState.Deleted);

        foreach (var entry in entries)
        {
            entry.State = EntityState.Unchanged;

            entry.Property(e => e.IsDeleted).CurrentValue = true;
            entry.Property(e => e.DeletedAt).CurrentValue = now;
            entry.Property(e => e.DeletedBy).CurrentValue = userId;
            entry.Property(e => e.IsDeleted).IsModified = true;
            entry.Property(e => e.DeletedAt).IsModified = true;
            entry.Property(e => e.DeletedBy).IsModified = true;

            if (entry.Entity is IAuditable)
            {
                entry.Property(nameof(IAuditable.UpdatedAt)).CurrentValue = now;
                entry.Property(nameof(IAuditable.UpdatedBy)).CurrentValue = userId;
                entry.Property(nameof(IAuditable.UpdatedAt)).IsModified = true;
                entry.Property(nameof(IAuditable.UpdatedBy)).IsModified = true;
            }
        }
    }
}
