using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;
using MediatR;
using System.Text.Json;

namespace ArquiveSe.Domain.Events;

public record DocumentFileUploaded : Event, INotification
{
    public DocumentFileUploaded() 
        : base(typeof(Document).FullName!, string.Empty)
    {
    }

    public DocumentFileUploaded(string documentId) 
        : base(typeof(Document).FullName!, documentId)
    {
    }

    public override string GetData() => JsonSerializer.Serialize(this);
}