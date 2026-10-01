using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace ChromaLoom.Infrastructure.Http.EndpointFilters;

public static class EndpointFilterExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        return builder
            .AddEndpointFilter<RequestValidationFilter<TRequest>>()
            .ProducesValidationProblem();
    }
}
