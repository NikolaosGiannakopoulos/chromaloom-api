using System.Linq.Expressions;
using ChromaLoom.Kernel.Abstractions.Entities;

namespace ChromaLoom.Infrastructure.Persistence.QueryFilters;

internal sealed class SoftDeleteQueryFilter : IQueryFilter
{
    public string Name => QueryFilterNames.SoftDelete;

    public bool AppliesTo(Type entityType)
    {
        return typeof(ISoftDeletable).IsAssignableFrom(entityType);
    }

    public LambdaExpression GetFilter(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "e");
        var property = Expression.Property(parameter, nameof(ISoftDeletable.IsDeleted));
        var body = Expression.Equal(property, Expression.Constant(false));
        return Expression.Lambda(body, parameter);
    }
}
