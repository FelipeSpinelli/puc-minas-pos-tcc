namespace ArquiveSe.App.Models.Responses;

public record GetDocumentByIdResponse
{
    public string Id { get; set; } = null!;
    public string Folder { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Base64 { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}