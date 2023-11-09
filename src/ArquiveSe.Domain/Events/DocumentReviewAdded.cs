using ArquiveSe.Domain.Entities;
using ArquiveSe.Domain.Shared;
using ArquiveSe.Domain.ValueObjects;

namespace ArquiveSe.Domain.Events;

public record DocumentReviewAdded : Event
{
    public Review Review { get; set; } = new();

    public DocumentReviewAdded()
        : base(typeof(Document).FullName!, string.Empty)
    {
    }

    public DocumentReviewAdded(
        string documentId,
        Review review)
        : base(typeof(Document).FullName!, documentId)
    {
        Review = review;
    }
}
