namespace Napoleon.Shared.Domain.Entity;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Ulid.NewUlid().ToGuid();

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public bool IsDeleted { get; set; }
}

