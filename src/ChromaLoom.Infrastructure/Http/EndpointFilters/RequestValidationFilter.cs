using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ChromaLoom.Infrastructure.Http.EndpointFilters;

internal sealed class RequestValidationFilter<TRequest> : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(next);

        var validator = context.HttpContext.RequestServices.GetService<IValidator<TRequest>>()
            ?? throw new InvalidOperationException(
                $"WithRequestValidation<{typeof(TRequest).Name}> requires a registered IValidator<{typeof(TRequest).Name}>.");

        var request = context.Arguments.OfType<TRequest>().FirstOrDefault()
            ?? throw new InvalidOperationException(
                $"Endpoint arguments do not contain a {typeof(TRequest).Name} parameter required for request validation.");

        var validationResult = await validator.ValidateAsync(
            request,
            context.HttpContext.RequestAborted);

        return validationResult.IsValid
            ? await next(context)
            : TypedResults.ValidationProblem(validationResult.ToDictionary());
    }
}
