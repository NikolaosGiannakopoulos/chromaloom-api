namespace ChromaLoom.Kernel.Exceptions;

public sealed class ForbiddenException : DomainException
{
    public ForbiddenException(string code, string message)
        : base(code, message)
    {
    }

    public ForbiddenException(string code, string message, Exception innerException)
        : base(code, message, innerException)
    {
    }
}
