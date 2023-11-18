using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;
using MediatR;
using System.Text.Json;

namespace ArquiveSe.Domain.Events;

public record FlowCreated : Event, INotification
{
    public string AccountId { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Permissions Permissions { get; set; } = Permissions.Empty;

    public FlowCreated() : base(typeof(Flow).FullName!, string.Empty)
    {
    }

    public FlowCreated(
        string flowId,
        string accountId,
        string externalId,
        string name,
        string description,
        Permissions permissions) : base(typeof(Folder).FullName!, flowId)
    {
        ExternalId = externalId;
        AccountId = accountId;
        Name = name;
        Description = description;
        Permissions = permissions;
    }

    public override string GetData() => JsonSerializer.Serialize(this);
}