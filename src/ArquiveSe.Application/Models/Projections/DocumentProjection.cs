using ArquiveSe.Application.Models.Dtos;

namespace ArquiveSe.Application.Models.Projections;

public class DocumentProjection
{
    public string Id { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public string FolderId { get; set; } = null!;
    public FolderSummaryDto Folder { get; set; } = new();
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
    public PermissionsDto Permissions { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}