namespace ArquiveSe.Application.Models.Outputs;

public record CreateDocumentOutput
{
    public string Id { get; set; } = null!;
    public string Code { get; set; } = null!;
}
