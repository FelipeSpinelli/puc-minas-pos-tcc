using ArquiveSe.Application.Models.Commands.Outputs;
using ArquiveSe.Application.Models.Dtos;
using MediatR;

namespace ArquiveSe.Application.Models.Commands.Inputs;
public record CreateDocumentInput : IRequest<CreateDocumentOutput>
{
    public string AccountId { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public string FolderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public PermissionsDto? CustomPermissions { get; set; }
    public bool InheritFolderPermissions { get; set; }
    public ulong ExpectedSize { get; set; }
}
