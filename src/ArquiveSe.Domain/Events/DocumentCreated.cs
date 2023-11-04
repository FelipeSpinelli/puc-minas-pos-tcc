using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Enumerators;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Events;
public record DocumentCreated : Event
{
    public string ExternalId { get; set; } = null!;
    public string FolderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public EDocumentType Type { get; set; }
    public Permissions Permissions { get; set; } = Permissions.Empty;
    public ulong ExpectedSize { get; set; }

    public DocumentCreated() : base(typeof(Document).FullName!, string.Empty)
    {
    }

    public DocumentCreated(
        string documentId,
        string externalId,
        string folderId,
        string name,
        string code,
        EDocumentType type,
        Permissions permissions,
        ulong expectedSize) : base(typeof(Folder).FullName!, documentId)
    {
        ExternalId = externalId;
        FolderId = folderId;
        Name = name;
        Code = code;
        Type = type;
        Permissions = permissions;
        ExpectedSize = expectedSize;
    }
}