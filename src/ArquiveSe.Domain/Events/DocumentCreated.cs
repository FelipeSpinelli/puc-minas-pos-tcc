using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Enumerators;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;
using MediatR;
using System.Text.Json;

namespace ArquiveSe.Domain.Events;
public record DocumentCreated : Event, INotification
{
    public string AccountId { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public string FolderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public EDocumentType Type { get; set; }
    public Permissions Permissions { get; set; } = Permissions.Empty;
    public ulong ExpectedSize { get; set; }

    public DocumentCreated() : base(typeof(Document).FullName!, string.Empty)
    {
    }

    public DocumentCreated(
        string documentId,
        string accountId,
        string externalId,
        string folderId,
        string name,
        EDocumentType type,
        Permissions permissions,
        ulong expectedSize) : base(typeof(Document).FullName!, documentId)
    {
        ExternalId = externalId;
        AccountId = accountId;
        FolderId = folderId;
        Name = name;
        Type = type;
        Permissions = permissions;
        ExpectedSize = expectedSize;
    }

    public override string GetData() => JsonSerializer.Serialize(this);
}