namespace ChromaLoom.Kernel.Abstractions.Entities;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
    DateTimeOffset? DeletedAt { get; }
    string? DeletedBy { get; }
}
