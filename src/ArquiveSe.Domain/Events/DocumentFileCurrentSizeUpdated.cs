using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;
using MediatR;
using System.Text.Json;

namespace ArquiveSe.Domain.Events;

public record DocumentFileUpdated : Event, INotification
{
    public ulong ChunkPosition { get; set; }
    public ulong SizeToAdd { get; set; }

    public DocumentFileUpdated() : base(typeof(Document).FullName!, string.Empty)
    {
    }

    public DocumentFileUpdated(
        string documentId,
        ulong chunkPosition,
        ulong sizeToAdd) : base(typeof(Document).FullName!, documentId)
    {
        ChunkPosition = chunkPosition;
        SizeToAdd = sizeToAdd;
    }

    public override string GetData() => JsonSerializer.Serialize(this);
}