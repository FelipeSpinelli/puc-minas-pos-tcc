using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Events;

public record FlowCreated : Event
{
    public string ExternalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Permissions Permissions { get; set; } = Permissions.Empty;

    public FlowCreated() : base(typeof(Flow).FullName!, string.Empty)
    {
    }

    public FlowCreated(
        string flowId,
        string externalId,
        string name,
        string description,
        Permissions permissions) : base(typeof(Folder).FullName!, flowId)
    {
        ExternalId = externalId;
        Name = name;
        Description = description;
        Permissions = permissions;
    }
}