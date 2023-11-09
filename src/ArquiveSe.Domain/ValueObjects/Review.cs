namespace ArquiveSe.Domain.ValueObjects;

public record Review
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string ReviewerId { get; set; } = null!;
    public bool RequiresChanges { get; set; }
    public string Comments { get; set; } = null!;
    public bool IsResolved { get; set; }
};