using ArquiveSe.Application.Models.Queries.Outputs;

namespace ArquiveSe.Api.Contracts.v1.Responses;

public class GetDocumentByIdResponse : 
    IFromOutputConverter<GetDocumentDetailOutput>,
    IFromOutputConverter<GetDocumentStreamOutput>
{
    public string Id { get; set; } = null!;
    public string Folder { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Base64 { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public void From(GetDocumentDetailOutput output)
    {
        Id = output.Id;
        Folder = output.Folder;
        Name = output.Name;
        Type = output.Type;
        Status = output.Status;
        CreatedAt = output.CreatedAt;
    }

    public void From(GetDocumentStreamOutput output)
    {
        Base64 = Convert.ToBase64String(output.Stream);
    }
}
