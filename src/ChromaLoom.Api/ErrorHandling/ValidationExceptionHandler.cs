using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ChromaLoom.Kernel.Results;
using ChromaLoom.Infrastructure.Http;
using Microsoft.AspNetCore.Diagnostics;

namespace ChromaLoom.Api.ErrorHandling;

internal sealed class ValidationExceptionHandler(
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);

        if (exception is not ValidationException validationException)
        {
            return false;
        }

        var (statusCode, title) = HttpErrorMapping.Map(ErrorType.Validation);
        httpContext.Response.StatusCode = statusCode;

        var errors = validationException.Errors
            .GroupBy(failure => failure.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(failure => failure.ErrorMessage).ToArray());

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ValidationProblemDetails(errors)
            {
                Title = title,
                Status = statusCode
            }
        });
    }
}
