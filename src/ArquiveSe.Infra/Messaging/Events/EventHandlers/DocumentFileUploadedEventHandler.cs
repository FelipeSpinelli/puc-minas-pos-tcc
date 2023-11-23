using ArquiveSe.Application.Models.Commands.Inputs;
using ArquiveSe.Application.Ports.Driving;
using ArquiveSe.Domain.Events;
using MediatR;

namespace ArquiveSe.Infra.Messaging.Events.EventHandlers;

public class DocumentFileUploadedEventHandler : INotificationHandler<DocumentFileUploaded>
{
    private readonly ICommandBusPort _bus;

    public DocumentFileUploadedEventHandler(ICommandBusPort bus)
    {
        _bus = bus;
    }

    public async Task Handle(DocumentFileUploaded notification, CancellationToken cancellationToken)
    {
        await _bus.Send(new JoinDocumentFileChunksInput { Id = notification.AggregateId });
    }
}