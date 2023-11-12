namespace ArquiveSe.Application.Models.Commands.Outputs;

public record CreateDocumentOutput
{
    public string Id { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
}
