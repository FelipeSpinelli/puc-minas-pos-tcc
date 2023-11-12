using ArquiveSe.Application.Models.Commands.Outputs;
using MediatR;

namespace ArquiveSe.Application.Models.Commands.Inputs;

public record AddDocumentFileChunkInput : IRequest<NoOutput>
{
    public string Id { get; set; } = null!;
    public int Position { get; set; }
    public string Base64Chunk { get; set; } = null!;

    public byte[] GetChuckBytes() => Convert.FromBase64String(Base64Chunk);
}