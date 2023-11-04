using ArquiveSe.Application.Models.Dtos;

namespace ArquiveSe.Application.Models.Projections;

public class FlowProjection
{
    public string Id { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public PermissionsDto Permissions { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
