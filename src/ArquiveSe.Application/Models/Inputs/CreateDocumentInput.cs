using ArquiveSe.Application.Models.Dtos;

namespace ArquiveSe.Application.Models.Inputs;
public record CreateDocumentInput
{
    public string ExternalId { get; set; } = null!;
    public string FolderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public PermissionsDto? CustomPermissions { get; set; }
    public bool InheritFolderPermissions { get; set; }
    public ulong ExpectedSize { get; set; }
}
