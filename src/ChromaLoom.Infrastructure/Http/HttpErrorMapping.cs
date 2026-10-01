using ChromaLoom.Kernel.Results;
using Microsoft.AspNetCore.Http;
using ChromaLoom.Kernel.Exceptions;

namespace ChromaLoom.Infrastructure.Http;

public static class HttpErrorMapping
{
    public static (int StatusCode, string Title) Map(ErrorType type)
    {
        return type switch
        {
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Validation failed."),
            ErrorType.Unauthorized => (StatusCodes.Status401Unauthorized, "Unauthorized."),
            ErrorType.Forbidden => (StatusCodes.Status403Forbidden, "Forbidden."),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Resource not found."),
            ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflict."),
            ErrorType.Failure => (StatusCodes.Status500InternalServerError, "An unexpected error occurred."),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };
    }

    public static (int StatusCode, string Title) Map(DomainException exception)
    {
        return exception switch
        {
            NotFoundException => Map(ErrorType.NotFound),
            ConflictException => Map(ErrorType.Conflict),
            ForbiddenException => Map(ErrorType.Forbidden),
            UnauthorizedException => Map(ErrorType.Unauthorized),
            _ => Map(ErrorType.Validation)
        };
    }
}
