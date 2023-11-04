namespace ArquiveSe.Domain.Shared;

public abstract class Entity
{
    public string Id { get; protected set; } = Guid.NewGuid().ToString("N");
    public string ExternalId { get; protected set; } = null!;
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    protected Entity(string externalId)
    {
        ExternalId = externalId;
    }
}