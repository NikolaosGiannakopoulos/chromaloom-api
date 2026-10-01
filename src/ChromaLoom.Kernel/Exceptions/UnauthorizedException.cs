namespace ChromaLoom.Kernel.Exceptions;

public sealed class UnauthorizedException : DomainException
{
    public UnauthorizedException(string code, string message)
        : base(code, message)
    {
    }

    public UnauthorizedException(string code, string message, Exception innerException)
        : base(code, message, innerException)
    {
    }
}
