using ArquiveSe.Domain.Enumerators;
using ArquiveSe.Domain.Events;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Entities;

public class Document : AggregateRoot
{
    public string AccountId { get; private set; } = null!;
    public string FolderId { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public EDocumentType Type { get; private set; }
    public EDocumentStatus Status { get; private set; }
    public Permissions Permissions { get; private set; } = Permissions.Empty;
    public FileData File { get; private set; } = new();
    public ICollection<Review> Reviews { get; private set; } = new List<Review>(0);

    public Document()
    {
    }

    public Document(
        string accountId,
        string externalId,
        string folderId,
        string name,
        EDocumentType type,
        Permissions permissions,
        ulong chunks,
        ulong expectedSize) : base(externalId)
    {
        var @event = new DocumentCreated
        (
            Id,
            accountId,
            externalId,
            folderId,
            name,
            type,
            permissions!,
            chunks,
            expectedSize
        );

        ApplyEvent(@event);
        RaiseEvent(@event);
    }

    public void UpdateFile(ulong chunkPosition, ulong sizeToAdd)
    {
        if (!CanApplyChunk(chunkPosition))
        {
            throw new ApplicationException($"Unexpected chunk position {chunkPosition}");
        }

        var currentSizeUpdatedEvent = new DocumentFileUpdated
        (
            Id,
            chunkPosition,
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

    public bool CanApplyChunk(ulong chunkPosition) => (long)chunkPosition == File!.CurrentChunks;

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
        Type = @event.Type;
        Permissions = @event.Permissions;
        File = new() { CurrentChunks = 0, ExpectedChunks = @event.Chunks, ExpectedSize = @event.ExpectedSize };
        Status = EDocumentStatus.Created;
    }

    protected void OnDocumentFileUpdated(DocumentFileUpdated @event)
    {
        File = File with
        {
            CurrentChunks = File.CurrentChunks + 1,
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