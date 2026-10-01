namespace ChromaLoom.Kernel.Results;

public sealed record Error(string Code, string Description, ErrorType Type)
{
    public static Error Validation(string code, string description)
    {
        return new(code, description, ErrorType.Validation);
    }

    public static Error Unauthorized(string code, string description)
    {
        return new(code, description, ErrorType.Unauthorized);
    }

    public static Error Forbidden(string code, string description)
    {
        return new(code, description, ErrorType.Forbidden);
    }

    public static Error NotFound(string code, string description)
    {
        return new(code, description, ErrorType.NotFound);
    }

    public static Error Conflict(string code, string description)
    {
        return new(code, description, ErrorType.Conflict);
    }

    public static Error Failure(string code, string description)
    {
        return new(code, description, ErrorType.Failure);
    }
}
