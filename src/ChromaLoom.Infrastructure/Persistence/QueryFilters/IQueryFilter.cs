using System.Linq.Expressions;

namespace ChromaLoom.Infrastructure.Persistence.QueryFilters;

internal interface IQueryFilter
{
    string Name { get; }
    bool AppliesTo(Type entityType);
    LambdaExpression GetFilter(Type entityType);
}
