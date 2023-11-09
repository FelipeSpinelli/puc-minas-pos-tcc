namespace ArquiveSe.Application.Models.Commands.Inputs;

public record AddDocumentReviewInput
{
    public string Id { get; set; } = null!;
    public string ReviewerId { get; set; } = null!;
    public bool RequiresChanges { get; set; }
    public string Comments { get; set; } = null!;
}
