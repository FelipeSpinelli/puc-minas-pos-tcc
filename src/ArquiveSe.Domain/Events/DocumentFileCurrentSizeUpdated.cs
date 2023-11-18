using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;
using MediatR;
using System.Text.Json;

namespace ArquiveSe.Domain.Events;

public record DocumentFileCurrentSizeUpdated : Event, INotification
{
    public ulong SizeToAdd { get; set; }

    public DocumentFileCurrentSizeUpdated() : base(typeof(Document).FullName!, string.Empty)
    {
    }

    public DocumentFileCurrentSizeUpdated(
        string documentId,
        ulong sizeToAdd) : base(typeof(Document).FullName!, documentId)
    {
        SizeToAdd = sizeToAdd;
    }

    public override string GetData() => JsonSerializer.Serialize(this);
}