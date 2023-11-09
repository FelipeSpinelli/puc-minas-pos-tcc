using ArquiveSe.Domain.Enumerators;
using ArquiveSe.Domain.Events;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Entities;

public class Document : AggregateRoot
{
    public string FolderId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public EDocumentType Type { get; private set; }
    public EDocumentStatus Status { get; private set; }
    public Permissions Permissions { get; private set; } = Permissions.Empty;
    public FileData File { get; private set; } = new();
    public ICollection<Review> Reviews { get; private set; } = new List<Review>(0);

    public Document()
    {
    }

    public Document(
        string externalId,
        string folderId,
        string folderCode,
        ushort sequential,
        string name,
        EDocumentType type,
        Permissions permissions,
        ulong expectedSize) : base(externalId)
    {
        var @event = new DocumentCreated
        (
            Id,
            externalId,
            folderId,
            name,
            $"{folderCode}-{sequential:0000}",
            type,
            permissions!,
            expectedSize
        );

        ApplyEvent(@event);
        RaiseEvent(@event);
    }

    public void UpdateFileCurrentSize(ulong sizeToAdd)
    {
        var currentSizeUpdatedEvent = new DocumentFileCurrentSizeUpdated
        (
            Id,
            sizeToAdd
        );

        ApplyEvent(currentSizeUpdatedEvent);
        RaiseEvent(currentSizeUpdatedEvent);

        if (!File.IsCompleted)
        {
            return;
        }

        var documentFileUploadedEvent = new DocumentFileUploaded(Id);
        ApplyEvent(documentFileUploadedEvent);
        RaiseEvent(documentFileUploadedEvent);
    }

    public void AddReview(Review review)
    {
        var @event = new DocumentReviewAdded
        (
            Id,
            review
        );

        ApplyEvent(@event);
        RaiseEvent(@event);
    }

    protected void OnDocumentCreated(DocumentCreated @event)
    {
        Id = @event.AggregateId;
        ExternalId = @event.ExternalId;
        FolderId = @event.FolderId;
        Name = @event.Name;
        Code = @event.Code;
        Type = @event.Type;
        Permissions = @event.Permissions;
        File = new() { ExpectedSize = @event.ExpectedSize };
        Status = EDocumentStatus.Created;
    }

    protected void OnDocumentFileCurrentSizeUpdated(DocumentFileCurrentSizeUpdated @event)
    {
        File = File with
        {
            CurrentSize = File.CurrentSize + @event.SizeToAdd
        };
    }

    protected void OnDocumentFileUploaded(DocumentFileUploaded @event)
    {
        Status = EDocumentStatus.Ready;
    }

    protected void OnDocumentReviewAdded(DocumentReviewAdded @event)
    {
        if (Status != EDocumentStatus.Ready)
        {
            throw new ApplicationException($"Documents with {Status} status cannot be reviewed!");
        }

        if (!Permissions.Reviewers.Contains(@event.Review.ReviewerId))
        {
            throw new ApplicationException($"{@event.Review.ReviewerId} is not a valid reviewer!");
        }

        Reviews.Add(@event.Review);

        Status = Reviews.All(x => x.IsResolved || !x.RequiresChanges) ? 
            EDocumentStatus.Reviewed : 
            EDocumentStatus.Ready;
    }
}