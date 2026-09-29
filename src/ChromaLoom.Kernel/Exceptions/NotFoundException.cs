namespace ChromaLoom.Kernel.Exceptions;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(string code, string message)
        : base(code, message)
    {
    }

    public NotFoundException(string code, string message, Exception innerException)
        : base(code, message, innerException)
    {
    }

    public static NotFoundException For(string resource, object key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(resource);
        ArgumentNullException.ThrowIfNull(key);

        return new("resource.not_found", $"{resource} '{key}' was not found.");
    }
}
