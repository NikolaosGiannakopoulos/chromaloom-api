using Microsoft.AspNetCore.Mvc;
using ChromaLoom.Kernel.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace ChromaLoom.Api.ErrorHandling;

internal sealed class DomainExceptionHandler(
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);

        if (exception is not DomainException domainException)
        {
            return false;
        }

        var statusCode = domainException switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            ConflictException => StatusCodes.Status409Conflict,
            ForbiddenException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status400BadRequest
        };

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = "Request could not be completed.",
                Detail = domainException.Message,
                Status = statusCode,
                Extensions =
                {
                    ["code"] = domainException.Code
                }
            }
        });
    }
}
