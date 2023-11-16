using ArquiveSe.Domain.Events;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Entities;

public class Flow : AggregateRoot
{
    public string AccountId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Permissions Permissions { get; private set; } = Permissions.Empty;

    public Flow()
    {
    }

    public Flow(
        string accountId,
        string externalId,
        string name,
        string description,
        Permissions permissions)
    {
        var @event = new FlowCreated
        (
            Id,
            accountId,
            externalId,
            name,
            description,
            permissions
        );
        ApplyEvent(@event);
        RaiseEvent(@event);
    }

    protected void OnFlowCreated(FlowCreated @event)
    {
        Id = @event.AggregateId;
        AccountId = @event.AccountId;
        ExternalId = @event.ExternalId;
        Name = @event.Name;
        Description = @event.Description;
        Permissions = @event.Permissions;
        CreatedAt = @event.Timestamp;
    }
}
