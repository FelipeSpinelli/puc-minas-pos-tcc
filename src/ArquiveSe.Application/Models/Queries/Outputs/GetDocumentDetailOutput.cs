using ArquiveSe.Application.Models.Projections;

namespace ArquiveSe.Application.Models.Queries.Outputs;

public class GetDocumentDetailOutput
{
    public string Id { get; set; } = null!;
    public string Folder { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public GetDocumentDetailOutput(DocumentProjection projection)
    {
        Id = projection.ExternalId;
        Folder = projection.Folder.Name;
        Name = projection.Name;
        Type = projection.Type;
        Status = projection.Status;
        CreatedAt = projection.CreatedAt;
    }
}