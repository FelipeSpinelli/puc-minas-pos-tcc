using ArquiveSe.Application.Models.Dtos;
using ArquiveSe.Application.Models.Projections;
using ArquiveSe.Application.Ports.Driven;
using ArquiveSe.Domain.Enumerators;
using ArquiveSe.Domain.Events;
using MediatR;

namespace ArquiveSe.Infra.Messaging.EventHandlers;

public class DocumentCreatedEventHandler : INotificationHandler<DocumentCreated>
{
    private readonly IDocumentReadDbPort _documentReadDbPort;
    private readonly IFolderReadDbPort _folderReadDbPort;

    public DocumentCreatedEventHandler(
        IDocumentReadDbPort documentReadDbPort, 
        IFolderReadDbPort folderReadDbPort)
    {
        _documentReadDbPort = documentReadDbPort;
        _folderReadDbPort = folderReadDbPort;
    }

    public async Task Handle(DocumentCreated notification, CancellationToken cancellationToken)
    {
        var folder = await _folderReadDbPort.GetFolderById(notification.FolderId);
        var documentProjection = new DocumentProjection
        {
            Id = notification.ExternalId,
            AccountId = notification.AccountId,
            FolderId = folder.Id,
            Name = notification.Name,
            Folder = new FolderSummaryDto
            {
                Name = notification.Name,
                Permissions = folder.Permissions
            },
            Permissions = new PermissionsDto
            {
                Approvers = notification.Permissions.Approvers,
                Reviewers = notification.Permissions.Reviewers
            },
            Status = EDocumentStatus.Created.ToString(),
            Type = notification.Type.ToString(),
            CreatedAt = notification.Timestamp
        };

        await _documentReadDbPort.Upsert(documentProjection);
    }
}