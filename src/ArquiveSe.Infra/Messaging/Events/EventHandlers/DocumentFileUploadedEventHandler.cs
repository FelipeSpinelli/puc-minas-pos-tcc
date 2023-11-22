using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Events;
using MediatR;

namespace ArquiveSe.Infra.Messaging.Events.EventHandlers;

public class DocumentFileUploadedEventHandler : INotificationHandler<DocumentFileUploaded>
{
    private readonly IPersistenceDbPort _persistenceDb;
    private readonly IFileStoragePort _fileStorage;

    public DocumentFileUploadedEventHandler(
        IPersistenceDbPort persistenceDb,
        IFileStoragePort fileStorage)
    {
        _persistenceDb = persistenceDb;
        _fileStorage = fileStorage;
    }

    public async Task Handle(DocumentFileUploaded notification, CancellationToken cancellationToken)
    {
        var document = await _persistenceDb.LoadAggregate<Document>(notification.AggregateId)
            ?? throw new ApplicationException($"Document {notification.AggregateId} was not found!");

        await _fileStorage.JoinChunks(document);
    }
}