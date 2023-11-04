namespace ArquiveSe.Application.Models.Dtos;

public record FolderSummaryDto
{
    public string Name { get; set; } = null!;
    public PermissionsDto Permissions { get; set; } = new();

    public FolderSummaryDto()
    {
    }
}