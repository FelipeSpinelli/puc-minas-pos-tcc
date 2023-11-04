using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;

namespace ArquiveSe.Domain.Events;

public record DocumentFileUploaded : Event
{
    public DocumentFileUploaded() : base(typeof(Document).FullName!, string.Empty)
    {
    }

    public DocumentFileUploaded(string documentId) 
        : base(typeof(Document).FullName!, documentId)
    {
    }
}
