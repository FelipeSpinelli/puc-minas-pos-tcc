using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Events;

public record FolderCreated : Event
{
    public string ExternalId { get; set; } = null!;
    public string FlowId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public Permissions Permissions { get; set; } = Permissions.Empty;
    public Folder? ParentFolder { get; set; }

    public FolderCreated() : base(typeof(Folder).FullName!, string.Empty)
    {            
    }

    public FolderCreated(
        string externalId,
        string folderId,
        string flowId,
        string name,
        string code,
        Permissions permissions,
        Folder? parentFolder) : base(typeof(Folder).FullName!, folderId)
    {
        ExternalId = externalId;
        FlowId = flowId;
        Name = name;
        Code = code;
        Permissions = permissions;
        ParentFolder = parentFolder;
    }
}