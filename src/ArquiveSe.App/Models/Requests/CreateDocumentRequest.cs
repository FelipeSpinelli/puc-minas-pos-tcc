using Microsoft.AspNetCore.Components.Forms;

namespace ArquiveSe.App.Models.Requests;

public record CreateDocumentRequest
{
    public string Token { get; set; } = null!;
    public string FolderId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Approvers { get; set; } = null!;
    public string Reviewers { get; set; } = null!;
    public bool InheritFolderPermissions { get; set; }
    public IBrowserFile? File { get; set; }
}
