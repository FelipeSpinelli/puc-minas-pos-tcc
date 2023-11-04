using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;

namespace ArquiveSe.Domain.Events;

public record DocumentFileCurrentSizeUpdated : Event
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
}