using ArquiveSe.Domain.Events;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Entities;

public class Folder : AggregateRoot
{
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public string FlowId { get; private set; } = null!;
    public Folder? Parent { get; private set; }
    public Permissions Permissions { get; private set; } = Permissions.Empty;

    public Folder()
    {
    }

    public Folder(
        string externalId,
        string flowId,
        string name,
        string codeSuffix,
        Permissions permissions,
        Folder? parentFolder = default)
    {
        var @event = new FolderCreated
        (
            Id,
            externalId,
            flowId,
            name,
            CreateCode(codeSuffix, parentFolder),
            permissions!,
            parentFolder
        );

        ApplyEvent(@event);
        RaiseEvent(@event);
    }

    protected void OnFolderCreated(FolderCreated @event)
    {
        Id = @event.AggregateId;
        ExternalId = @event.ExternalId;
        FlowId = @event.FlowId;
        Name = @event.Name;
        Code = @event.Code;
        Parent = @event.ParentFolder;
        Permissions = @event.Permissions;
        CreatedAt = @event.Timestamp;
    }

    public static string CreateCode(string codeSuffix, Folder? folder)
    {
        if (folder is null)
        {
            return codeSuffix;
        }

        return $"{folder.Code}-{codeSuffix}";
    }
}
