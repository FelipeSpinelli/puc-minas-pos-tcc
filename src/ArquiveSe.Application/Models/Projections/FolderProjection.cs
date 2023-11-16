using ArquiveSe.Application.Models.Dtos;

namespace ArquiveSe.Application.Models.Projections;

public class FolderProjection
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string FlowId { get; set; } = null!;
    public string? ParentId { get; set; }
    public FolderSummaryDto? Parent { get; set; }
    public PermissionsDto Permissions { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}