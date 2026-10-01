using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChromaLoom.Kernel.Results;
using ChromaLoom.Infrastructure.Http;
using Microsoft.AspNetCore.Diagnostics;
using ChromaLoom.Kernel.Abstractions.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ChromaLoom.Api.ErrorHandling;

internal sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);

        var method = httpContext.Request.Method;
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        var userId = httpContext.RequestServices.GetService<ICurrentUser>()?.Id ?? "anonymous";

        Logs.Unhandled(logger, exception, method, path, traceId, userId);

        var (statusCode, title) = HttpErrorMapping.Map(ErrorType.Failure);
        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode
            }
        });
    }
}

internal static partial class Logs
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Unhandled exception {Method} {Path} traceId={TraceId} user={UserId}")]
    internal static partial void Unhandled(
        ILogger logger,
        Exception exception,
        string method,
        string path,
        string traceId,
        string userId);
}
