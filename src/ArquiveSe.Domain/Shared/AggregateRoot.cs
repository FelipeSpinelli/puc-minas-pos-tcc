namespace ArquiveSe.Domain.Shared;

public abstract class AggregateRoot : Entity
{
    private readonly Queue<Event> _eventsQueue = new();

    public AggregateRoot(string externalId) : base(externalId)
    {
    }

    public AggregateRoot() : this(Guid.NewGuid().ToString("N"))
    {
    }

    public virtual void ApplyEvent(Event @event)
    {
        var type = GetType();
        var applyEventMethod = type.GetMethods().Single(x => x.Name.Equals($"On{@event.GetType().Name}"))!;

        applyEventMethod.Invoke(this, new object[] { @event });
    }

    protected void RaiseEvent(Event @event) => _eventsQueue.Enqueue(@event);
    public bool TryGetEvent(out Event @event) => _eventsQueue.TryDequeue(out @event);
}
