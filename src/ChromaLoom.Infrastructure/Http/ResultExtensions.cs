using ChromaLoom.Kernel.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ChromaLoom.Infrastructure.Http;

public static class ResultExtensions
{
    public static IResult ToHttpResult(
        this Result result,
        Func<IResult>? onSuccess = null)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.Match(
            onSuccess: () => onSuccess?.Invoke() ?? TypedResults.NoContent(),
            onFailure: ToProblemResult);
    }

    public static IResult ToHttpResult<TValue>(
        this Result<TValue> result,
        Func<TValue, IResult> onSuccess)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(onSuccess);

        return result.Match(
            onSuccess: onSuccess,
            onFailure: ToProblemResult);
    }

    public static ProblemHttpResult ToProblemResult(this Result result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess
            ? throw new InvalidOperationException("Cannot convert a successful result to a problem result.")
            : ToProblemResult(result.Error!);
    }

    public static ProblemHttpResult ToProblemResult(this Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        var (statusCode, title) = error.Type switch
        {
            ErrorType.Validation => (StatusCodes.Status400BadRequest, "Validation failed."),
            ErrorType.Unauthorized => (StatusCodes.Status401Unauthorized, "Unauthorized."),
            ErrorType.Forbidden => (StatusCodes.Status403Forbidden, "Forbidden."),
            ErrorType.NotFound => (StatusCodes.Status404NotFound, "Resource not found."),
            ErrorType.Conflict => (StatusCodes.Status409Conflict, "Conflict."),
            _ => (StatusCodes.Status400BadRequest, "Request could not be completed.")
        };

        return TypedResults.Problem(
            title: title,
            detail: error.Description,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                ["code"] = error.Code
            });
    }
}
