namespace ChromaLoom.Kernel.Results;

public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error is not null)
        {
            throw new InvalidOperationException("Success results cannot contain an error.");
        }

        if (!isSuccess && error is null)
        {
            throw new InvalidOperationException("Failure results must provide an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error? Error { get; }

    public static Result Success()
    {
        return new(true, null);
    }

    public static Result Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(false, error);
    }

    public static implicit operator Result(Error error)
    {
        return Failure(error);
    }

    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
}

public sealed class Result<TValue> : Result
{
    private Result(TValue? value, bool isSuccess, Error? error)
        : base(isSuccess, error)
    {
        if (isSuccess && value is null)
        {
            throw new ArgumentNullException(nameof(value), "Success results must contain a value.");
        }

        Value = value!;
    }

    public TValue Value
    {
        get => IsSuccess
            ? field
            : throw new InvalidOperationException("Cannot access the value of a failed result.");
        private init;
    }

    public static Result<TValue> Success(TValue value)
    {
        return new(value, true, null);
    }

    public static new Result<TValue> Failure(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(default, false, error);
    }

    public static implicit operator Result<TValue>(TValue value)
    {
        return Success(value);
    }

    public static implicit operator Result<TValue>(Error error)
    {
        return Failure(error);
    }

    public TResult Match<TResult>(
        Func<TValue, TResult> onSuccess,
        Func<Error, TResult> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);

        return IsSuccess ? onSuccess(Value) : onFailure(Error!);
    }
}
