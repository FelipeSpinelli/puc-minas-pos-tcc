namespace ArquiveSe.Domain.Shared;

public abstract record Event
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public virtual string EventType => GetType().FullName ?? GetType().Name;
    public string AggregateType { get; set; } = null!;
    public string AggregateId { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public bool MustNotBeRaised { get; private set; }
    public abstract string GetData();

    protected Event(string aggregateType, string aggregateId)
    {
        AggregateType = aggregateType;
        AggregateId = aggregateId;
    }

    public void MarkToNotBeRaised() => MustNotBeRaised = true;
}