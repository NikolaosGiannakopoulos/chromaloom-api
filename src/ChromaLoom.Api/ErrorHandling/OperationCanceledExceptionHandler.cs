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

        if (exception is not OperationCanceledException
            || !httpContext.RequestAborted.IsCancellationRequested)
        {
            return ValueTask.FromResult(false);
        }

        return ValueTask.FromResult(true);
    }
}
