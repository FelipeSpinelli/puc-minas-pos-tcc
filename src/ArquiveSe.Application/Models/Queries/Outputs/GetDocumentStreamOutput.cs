namespace ArquiveSe.Application.Models.Queries.Outputs;

public class GetDocumentStreamOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set;} = null!;
    public byte[] Stream { get; set; } = Array.Empty<byte>();
}
