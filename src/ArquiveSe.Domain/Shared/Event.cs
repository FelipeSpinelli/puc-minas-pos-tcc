namespace ArquiveSe.Domain.Shared;

public abstract record Event
{
    public string Id { get; private set; } = Guid.NewGuid().ToString("N");
    public string AggregateType { get; private set; } = null!;
    public string AggregateId { get; private set; } = null!;
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    protected Event(string aggregateType, string aggregateId)
    {
        AggregateType = aggregateType;
        AggregateId = aggregateId;
    }
}