using Microsoft.AspNetCore.Diagnostics;

namespace ChromaLoom.Api.ErrorHandling;

internal sealed class OperationCanceledExceptionHandler : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);

        return ValueTask.FromResult(
            exception is OperationCanceledException
            && httpContext.RequestAborted.IsCancellationRequested);
    }
}
